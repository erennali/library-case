import { useState, useEffect } from 'react'
import { useSearchParams } from 'react-router-dom'
import { useQuery } from '@tanstack/react-query'
import { searchApi } from '@/services/api'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card'
import Button from '@/components/ui/Button'
import Input from '@/components/ui/Input'
import Badge from '@/components/ui/Badge'
import LoadingSpinner from '@/components/ui/LoadingSpinner'
import { MagnifyingGlassIcon, BookOpenIcon, UsersIcon } from '@heroicons/react/24/outline'
import { Link } from 'react-router-dom'

export default function SearchPage() {
  const [searchParams, setSearchParams] = useSearchParams()
  const [query, setQuery] = useState(searchParams.get('q') || '')
  const [searchType, setSearchType] = useState<'global' | 'books' | 'members'>('global')
  const [page, setPage] = useState(1)

  const { data: searchResults, isLoading, refetch } = useQuery({
    queryKey: ['search', query, searchType, page],
    queryFn: () => {
      if (!query.trim()) return null
      
      switch (searchType) {
        case 'books':
          return searchApi.searchBooks(query, { page, pageSize: 20 })
        case 'members':
          return searchApi.searchMembers(query, { page, pageSize: 20 })
        default:
          return searchApi.globalSearch(query, { page, pageSize: 20 })
      }
    },
    enabled: !!query.trim(),
  })

  const { data: suggestions } = useQuery({
    queryKey: ['search-suggestions', query],
    queryFn: () => searchApi.getSuggestions(query, { maxResults: 5 }),
    enabled: query.length > 2,
  })

  const { data: popularSearches } = useQuery({
    queryKey: ['popular-searches'],
    queryFn: () => searchApi.getPopularSearches({ maxResults: 10 }),
  })

  useEffect(() => {
    const queryParam = searchParams.get('q')
    if (queryParam) {
      setQuery(queryParam)
    }
  }, [searchParams])

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault()
    if (query.trim()) {
      setSearchParams({ q: query.trim() })
      setPage(1)
      refetch()
    }
  }

  const handleSuggestionClick = (suggestion: string) => {
    setQuery(suggestion)
    setSearchParams({ q: suggestion })
    refetch()
  }

  const searchTypes = [
    { key: 'global', label: 'All', icon: MagnifyingGlassIcon },
    { key: 'books', label: 'Books', icon: BookOpenIcon },
    { key: 'members', label: 'Members', icon: UsersIcon },
  ]

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-gray-900">Search</h1>
        <p className="mt-1 text-sm text-gray-500">
          Find books, members, and other library resources
        </p>
      </div>

      {/* Search Form */}
      <Card>
        <CardContent className="p-6">
          <form onSubmit={handleSearch} className="space-y-4">
            <div className="flex gap-4">
              <div className="flex-1">
                <Input
                  placeholder="Search for books, members, authors, ISBN..."
                  value={query}
                  onChange={(e) => setQuery(e.target.value)}
                  className="w-full"
                />
              </div>
              <Button type="submit">
                <MagnifyingGlassIcon className="h-4 w-4 mr-2" />
                Search
              </Button>
            </div>

            {/* Search Type Tabs */}
            <div className="flex space-x-1 bg-gray-100 p-1 rounded-lg">
              {searchTypes.map((type) => (
                <button
                  key={type.key}
                  type="button"
                  onClick={() => setSearchType(type.key as any)}
                  className={`flex items-center space-x-2 px-3 py-2 rounded-md text-sm font-medium transition-colors ${
                    searchType === type.key
                      ? 'bg-white text-primary-600 shadow-sm'
                      : 'text-gray-600 hover:text-gray-900'
                  }`}
                >
                  <type.icon className="h-4 w-4" />
                  <span>{type.label}</span>
                </button>
              ))}
            </div>
          </form>

          {/* Search Suggestions */}
          {suggestions && suggestions.length > 0 && query.length > 2 && (
            <div className="mt-4">
              <p className="text-sm font-medium text-gray-700 mb-2">Suggestions:</p>
              <div className="flex flex-wrap gap-2">
                {suggestions.map((suggestion, index) => (
                  <button
                    key={index}
                    onClick={() => handleSuggestionClick(suggestion)}
                    className="px-3 py-1 bg-gray-100 hover:bg-gray-200 rounded-full text-sm text-gray-700 transition-colors"
                  >
                    {suggestion}
                  </button>
                ))}
              </div>
            </div>
          )}
        </CardContent>
      </Card>

      {/* Search Results */}
      {query && (
        <Card>
          <CardHeader>
            <CardTitle>
              Search Results for "{query}" 
              {searchResults && (
                <span className="text-sm font-normal text-gray-500 ml-2">
                  ({searchResults.totalResults} results)
                </span>
              )}
            </CardTitle>
          </CardHeader>
          <CardContent>
            {isLoading ? (
              <div className="flex items-center justify-center h-32">
                <LoadingSpinner size="lg" />
              </div>
            ) : searchResults && searchResults.results ? (
              <div className="space-y-4">
                {searchResults.results.map((result) => (
                  <div key={`${result.type}-${result.id}`} className="border border-gray-200 rounded-lg p-4 hover:bg-gray-50 transition-colors">
                    <div className="flex items-start justify-between">
                      <div className="flex-1">
                        <div className="flex items-center space-x-2 mb-2">
                          <Badge variant="info">{result.type}</Badge>
                          {result.type === 'Book' && (
                            <BookOpenIcon className="h-4 w-4 text-gray-400" />
                          )}
                          {result.type === 'Member' && (
                            <UsersIcon className="h-4 w-4 text-gray-400" />
                          )}
                        </div>
                        
                        <h3 className="text-lg font-medium text-gray-900 mb-1">
                          {result.type === 'Book' ? (
                            <Link to={`/books/${result.id}`} className="hover:text-primary-600">
                              {result.title}
                            </Link>
                          ) : result.type === 'Member' ? (
                            <Link to={`/members/${result.id}`} className="hover:text-primary-600">
                              {result.title}
                            </Link>
                          ) : (
                            result.title
                          )}
                        </h3>
                        
                        <p className="text-sm text-gray-600 mb-2">{result.description}</p>
                        
                        {result.metadata && (
                          <div className="flex flex-wrap gap-2">
                            {Object.entries(result.metadata).map(([key, value]) => (
                              <span key={key} className="text-xs text-gray-500">
                                {key}: {String(value)}
                              </span>
                            ))}
                          </div>
                        )}
                      </div>
                      
                      {result.imageUrl && (
                        <img
                          src={result.imageUrl}
                          alt={result.title}
                          className="h-16 w-12 object-cover rounded ml-4"
                        />
                      )}
                    </div>
                  </div>
                ))}
              </div>
            ) : (
              <div className="text-center py-12">
                <MagnifyingGlassIcon className="h-12 w-12 text-gray-400 mx-auto mb-4" />
                <h3 className="text-lg font-medium text-gray-900">No results found</h3>
                <p className="text-gray-500">Try adjusting your search terms or search type.</p>
              </div>
            )}
          </CardContent>
        </Card>
      )}

      {/* Popular Searches */}
      {!query && popularSearches && popularSearches.length > 0 && (
        <Card>
          <CardHeader>
            <CardTitle>Popular Searches</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-5 gap-3">
              {popularSearches.map((search, index) => (
                <button
                  key={index}
                  onClick={() => handleSuggestionClick(search.query)}
                  className="p-3 text-left border border-gray-200 rounded-lg hover:bg-gray-50 transition-colors"
                >
                  <p className="font-medium text-gray-900">{search.query}</p>
                  <p className="text-xs text-gray-500">{search.count} searches</p>
                </button>
              ))}
            </div>
          </CardContent>
        </Card>
      )}
    </div>
  )
}