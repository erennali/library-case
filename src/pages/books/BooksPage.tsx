import { useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { Link } from 'react-router-dom'
import { PlusIcon, MagnifyingGlassIcon } from '@heroicons/react/24/outline'
import { booksApi, categoriesApi } from '@/services/api'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/Table'
import Button from '@/components/ui/Button'
import Input from '@/components/ui/Input'
import Badge from '@/components/ui/Badge'
import Pagination from '@/components/ui/Pagination'
import LoadingSpinner from '@/components/ui/LoadingSpinner'
import BookForm from '@/components/forms/BookForm'
import Modal from '@/components/ui/Modal'
import { Book, BookStatus } from '@/types'

export default function BooksPage() {
  const [page, setPage] = useState(1)
  const [pageSize] = useState(10)
  const [search, setSearch] = useState('')
  const [categoryId, setCategoryId] = useState<number | undefined>()
  const [showCreateModal, setShowCreateModal] = useState(false)
  const [editingBook, setEditingBook] = useState<Book | null>(null)

  const { data: booksData, isLoading, refetch } = useQuery({
    queryKey: ['books', page, pageSize, search, categoryId],
    queryFn: () => booksApi.getAll({ page, pageSize, search, categoryId }),
  })

  const { data: categoriesData } = useQuery({
    queryKey: ['categories'],
    queryFn: () => categoriesApi.getAll({ pageSize: 100 }),
  })

  const getStatusBadge = (status: BookStatus) => {
    const variants = {
      [BookStatus.Available]: 'success',
      [BookStatus.Borrowed]: 'info',
      [BookStatus.Reserved]: 'warning',
      [BookStatus.OutOfStock]: 'error',
      [BookStatus.Discontinued]: 'default',
      [BookStatus.UnderMaintenance]: 'warning',
    } as const

    return <Badge variant={variants[status]}>{status}</Badge>
  }

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault()
    setPage(1)
    refetch()
  }

  const handleBookSaved = () => {
    setShowCreateModal(false)
    setEditingBook(null)
    refetch()
  }

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-64">
        <LoadingSpinner size="lg" />
      </div>
    )
  }

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Books</h1>
          <p className="mt-1 text-sm text-gray-500">
            Manage your library's book collection
          </p>
        </div>
        <Button onClick={() => setShowCreateModal(true)}>
          <PlusIcon className="h-4 w-4 mr-2" />
          Add Book
        </Button>
      </div>

      {/* Filters */}
      <Card>
        <CardContent className="p-6">
          <form onSubmit={handleSearch} className="flex gap-4">
            <div className="flex-1">
              <Input
                placeholder="Search books by title, author, or ISBN..."
                value={search}
                onChange={(e) => setSearch(e.target.value)}
                className="w-full"
              />
            </div>
            <div className="w-48">
              <select
                className="input"
                value={categoryId || ''}
                onChange={(e) => setCategoryId(e.target.value ? Number(e.target.value) : undefined)}
              >
                <option value="">All Categories</option>
                {categoriesData?.items.map((category) => (
                  <option key={category.id} value={category.id}>
                    {category.name}
                  </option>
                ))}
              </select>
            </div>
            <Button type="submit" variant="outline">
              <MagnifyingGlassIcon className="h-4 w-4 mr-2" />
              Search
            </Button>
          </form>
        </CardContent>
      </Card>

      {/* Books Table */}
      <Card>
        <CardHeader>
          <CardTitle>Books ({booksData?.totalCount || 0})</CardTitle>
        </CardHeader>
        <CardContent className="p-0">
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Book</TableHead>
                <TableHead>Author</TableHead>
                <TableHead>Category</TableHead>
                <TableHead>Copies</TableHead>
                <TableHead>Status</TableHead>
                <TableHead>Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {booksData?.items.map((book) => (
                <TableRow key={book.id}>
                  <TableCell>
                    <div className="flex items-center space-x-3">
                      {book.imageUrl && (
                        <img
                          src={book.imageUrl}
                          alt={book.title}
                          className="h-12 w-8 object-cover rounded"
                        />
                      )}
                      <div>
                        <Link
                          to={`/books/${book.id}`}
                          className="font-medium text-gray-900 hover:text-primary-600"
                        >
                          {book.title}
                        </Link>
                        <p className="text-sm text-gray-500">ISBN: {book.isbn}</p>
                      </div>
                    </div>
                  </TableCell>
                  <TableCell>{book.author}</TableCell>
                  <TableCell>{book.category?.name}</TableCell>
                  <TableCell>
                    <span className="text-sm">
                      {book.availableCopies}/{book.totalCopies}
                    </span>
                  </TableCell>
                  <TableCell>{getStatusBadge(book.status)}</TableCell>
                  <TableCell>
                    <div className="flex space-x-2">
                      <Button
                        size="sm"
                        variant="outline"
                        onClick={() => setEditingBook(book)}
                      >
                        Edit
                      </Button>
                      <Link to={`/books/${book.id}`}>
                        <Button size="sm" variant="ghost">
                          View
                        </Button>
                      </Link>
                    </div>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>

          {booksData && (
            <Pagination
              currentPage={page}
              totalPages={Math.ceil(booksData.totalCount / pageSize)}
              onPageChange={setPage}
              totalItems={booksData.totalCount}
              itemsPerPage={pageSize}
            />
          )}
        </CardContent>
      </Card>

      {/* Create/Edit Modal */}
      <Modal
        open={showCreateModal || !!editingBook}
        onClose={() => {
          setShowCreateModal(false)
          setEditingBook(null)
        }}
        title={editingBook ? 'Edit Book' : 'Add New Book'}
        size="lg"
      >
        <BookForm
          book={editingBook}
          onSave={handleBookSaved}
          onCancel={() => {
            setShowCreateModal(false)
            setEditingBook(null)
          }}
        />
      </Modal>
    </div>
  )
}