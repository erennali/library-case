import { useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { statisticsApi } from '@/services/api'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card'
import LoadingSpinner from '@/components/ui/LoadingSpinner'
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, LineChart, Line, PieChart, Pie, Cell, AreaChart, Area } from 'recharts'

export default function StatisticsPage() {
  const [dateRange, setDateRange] = useState({
    fromDate: new Date(Date.now() - 30 * 24 * 60 * 60 * 1000).toISOString().split('T')[0],
    toDate: new Date().toISOString().split('T')[0],
  })

  const { data: overview, isLoading } = useQuery({
    queryKey: ['statistics-overview'],
    queryFn: statisticsApi.getOverview,
  })

  const { data: circulationStats } = useQuery({
    queryKey: ['circulation-statistics', dateRange],
    queryFn: () => statisticsApi.getCirculationStatistics(dateRange),
  })

  const { data: bookStats } = useQuery({
    queryKey: ['book-statistics', dateRange],
    queryFn: () => statisticsApi.getBookStatistics(dateRange),
  })

  const { data: memberStats } = useQuery({
    queryKey: ['member-statistics', dateRange],
    queryFn: () => statisticsApi.getMemberStatistics(dateRange),
  })

  const { data: topBooks } = useQuery({
    queryKey: ['top-books', dateRange],
    queryFn: () => statisticsApi.getTopBooks({ topCount: 10, ...dateRange }),
  })

  const { data: topMembers } = useQuery({
    queryKey: ['top-members', dateRange],
    queryFn: () => statisticsApi.getTopMembers({ topCount: 10, ...dateRange }),
  })

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-64">
        <LoadingSpinner size="lg" />
      </div>
    )
  }

  // Mock data for charts
  const circulationData = [
    { date: '2024-01-01', checkouts: 45, returns: 42, renewals: 8 },
    { date: '2024-01-02', checkouts: 52, returns: 48, renewals: 12 },
    { date: '2024-01-03', checkouts: 38, returns: 45, renewals: 6 },
    { date: '2024-01-04', checkouts: 61, returns: 55, renewals: 15 },
    { date: '2024-01-05', checkouts: 49, returns: 51, renewals: 9 },
    { date: '2024-01-06', checkouts: 44, returns: 47, renewals: 7 },
    { date: '2024-01-07', checkouts: 35, returns: 38, renewals: 5 },
  ]

  const categoryData = [
    { name: 'Fiction', value: 400, color: '#3B82F6' },
    { name: 'Science', value: 300, color: '#10B981' },
    { name: 'History', value: 200, color: '#F59E0B' },
    { name: 'Technology', value: 150, color: '#EF4444' },
    { name: 'Arts', value: 100, color: '#8B5CF6' },
  ]

  const membershipData = [
    { type: 'Regular', count: 450 },
    { type: 'Student', count: 320 },
    { type: 'Faculty', count: 180 },
    { type: 'Staff', count: 95 },
    { type: 'Premium', count: 65 },
  ]

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Statistics</h1>
          <p className="mt-1 text-sm text-gray-500">
            Comprehensive analytics and insights for your library
          </p>
        </div>
        
        {/* Date Range Selector */}
        <div className="flex items-center space-x-4">
          <div>
            <label className="block text-xs font-medium text-gray-700 mb-1">From</label>
            <input
              type="date"
              value={dateRange.fromDate}
              onChange={(e) => setDateRange(prev => ({ ...prev, fromDate: e.target.value }))}
              className="input text-sm"
            />
          </div>
          <div>
            <label className="block text-xs font-medium text-gray-700 mb-1">To</label>
            <input
              type="date"
              value={dateRange.toDate}
              onChange={(e) => setDateRange(prev => ({ ...prev, toDate: e.target.value }))}
              className="input text-sm"
            />
          </div>
        </div>
      </div>

      {/* Overview Stats */}
      {overview && (
        <div className="grid grid-cols-2 md:grid-cols-4 lg:grid-cols-6 gap-4">
          <Card>
            <CardContent className="p-4 text-center">
              <p className="text-2xl font-bold text-blue-600">{overview.totalBooks}</p>
              <p className="text-sm text-gray-600">Total Books</p>
            </CardContent>
          </Card>
          <Card>
            <CardContent className="p-4 text-center">
              <p className="text-2xl font-bold text-green-600">{overview.totalMembers}</p>
              <p className="text-sm text-gray-600">Total Members</p>
            </CardContent>
          </Card>
          <Card>
            <CardContent className="p-4 text-center">
              <p className="text-2xl font-bold text-purple-600">{overview.totalTransactions}</p>
              <p className="text-sm text-gray-600">Transactions</p>
            </CardContent>
          </Card>
          <Card>
            <CardContent className="p-4 text-center">
              <p className="text-2xl font-bold text-red-600">{overview.overdueItems}</p>
              <p className="text-sm text-gray-600">Overdue Items</p>
            </CardContent>
          </Card>
          <Card>
            <CardContent className="p-4 text-center">
              <p className="text-2xl font-bold text-yellow-600">${overview.totalFines}</p>
              <p className="text-sm text-gray-600">Total Fines</p>
            </CardContent>
          </Card>
          <Card>
            <CardContent className="p-4 text-center">
              <p className="text-2xl font-bold text-indigo-600">{overview.activeReservations}</p>
              <p className="text-sm text-gray-600">Reservations</p>
            </CardContent>
          </Card>
        </div>
      )}

      {/* Charts Grid */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Daily Circulation */}
        <Card>
          <CardHeader>
            <CardTitle>Daily Circulation</CardTitle>
          </CardHeader>
          <CardContent>
            <ResponsiveContainer width="100%" height={300}>
              <AreaChart data={circulationData}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="date" tickFormatter={(value) => new Date(value).toLocaleDateString()} />
                <YAxis />
                <Tooltip labelFormatter={(value) => new Date(value).toLocaleDateString()} />
                <Area type="monotone" dataKey="checkouts" stackId="1" stroke="#3B82F6" fill="#3B82F6" fillOpacity={0.6} />
                <Area type="monotone" dataKey="returns" stackId="1" stroke="#10B981" fill="#10B981" fillOpacity={0.6} />
                <Area type="monotone" dataKey="renewals" stackId="1" stroke="#F59E0B" fill="#F59E0B" fillOpacity={0.6} />
              </AreaChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>

        {/* Books by Category */}
        <Card>
          <CardHeader>
            <CardTitle>Books by Category</CardTitle>
          </CardHeader>
          <CardContent>
            <ResponsiveContainer width="100%" height={300}>
              <PieChart>
                <Pie
                  data={categoryData}
                  cx="50%"
                  cy="50%"
                  outerRadius={80}
                  fill="#8884d8"
                  dataKey="value"
                  label={({ name, percent }) => `${name} ${(percent * 100).toFixed(0)}%`}
                >
                  {categoryData.map((entry, index) => (
                    <Cell key={`cell-${index}`} fill={entry.color} />
                  ))}
                </Pie>
                <Tooltip />
              </PieChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>

        {/* Member Distribution */}
        <Card>
          <CardHeader>
            <CardTitle>Members by Type</CardTitle>
          </CardHeader>
          <CardContent>
            <ResponsiveContainer width="100%" height={300}>
              <BarChart data={membershipData}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="type" />
                <YAxis />
                <Tooltip />
                <Bar dataKey="count" fill="#3B82F6" />
              </BarChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>

        {/* Monthly Trends */}
        <Card>
          <CardHeader>
            <CardTitle>Monthly Trends</CardTitle>
          </CardHeader>
          <CardContent>
            <ResponsiveContainer width="100%" height={300}>
              <LineChart data={circulationData}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="date" tickFormatter={(value) => new Date(value).toLocaleDateString()} />
                <YAxis />
                <Tooltip labelFormatter={(value) => new Date(value).toLocaleDateString()} />
                <Line type="monotone" dataKey="checkouts" stroke="#3B82F6" strokeWidth={2} />
                <Line type="monotone" dataKey="returns" stroke="#10B981" strokeWidth={2} />
              </LineChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>
      </div>

      {/* Top Lists */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Top Books */}
        <Card>
          <CardHeader>
            <CardTitle>Most Popular Books</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="space-y-3">
              {topBooks?.slice(0, 10).map((book, index) => (
                <div key={book.bookId} className="flex items-center justify-between p-3 bg-gray-50 rounded-lg">
                  <div className="flex items-center space-x-3">
                    <span className="flex items-center justify-center w-6 h-6 bg-primary-600 text-white text-xs font-bold rounded-full">
                      {index + 1}
                    </span>
                    <div>
                      <Link to={`/books/${book.bookId}`} className="font-medium text-gray-900 hover:text-primary-600">
                        {book.bookTitle}
                      </Link>
                      <p className="text-sm text-gray-500">{book.author}</p>
                    </div>
                  </div>
                  <div className="text-right">
                    <p className="text-sm font-medium">{book.checkoutCount} checkouts</p>
                    <p className="text-xs text-gray-500">{book.reservationCount} reservations</p>
                  </div>
                </div>
              )) || (
                <p className="text-gray-500 text-center py-4">No data available</p>
              )}
            </div>
          </CardContent>
        </Card>

        {/* Top Members */}
        <Card>
          <CardHeader>
            <CardTitle>Most Active Members</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="space-y-3">
              {topMembers?.slice(0, 10).map((member, index) => (
                <div key={member.memberId} className="flex items-center justify-between p-3 bg-gray-50 rounded-lg">
                  <div className="flex items-center space-x-3">
                    <span className="flex items-center justify-center w-6 h-6 bg-green-600 text-white text-xs font-bold rounded-full">
                      {index + 1}
                    </span>
                    <div>
                      <Link to={`/members/${member.memberId}`} className="font-medium text-gray-900 hover:text-primary-600">
                        {member.memberName}
                      </Link>
                      <p className="text-sm text-gray-500">
                        Member since {new Date(member.membershipStartDate).toLocaleDateString()}
                      </p>
                    </div>
                  </div>
                  <div className="text-right">
                    <p className="text-sm font-medium">{member.checkoutCount} books</p>
                    <p className="text-xs text-gray-500">${member.totalFines} fines</p>
                  </div>
                </div>
              )) || (
                <p className="text-gray-500 text-center py-4">No data available</p>
              )}
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  )
}