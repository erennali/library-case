import { useQuery } from '@tanstack/react-query'
import { dashboardApi } from '@/services/api'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card'
import LoadingSpinner from '@/components/ui/LoadingSpinner'
import {
  BookOpenIcon,
  UsersIcon,
  ArrowsRightLeftIcon,
  ExclamationTriangleIcon,
  CurrencyDollarIcon,
  CalendarDaysIcon,
} from '@heroicons/react/24/outline'
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, LineChart, Line, PieChart, Pie, Cell } from 'recharts'

export default function DashboardPage() {
  const { data: overview, isLoading } = useQuery({
    queryKey: ['dashboard-overview'],
    queryFn: dashboardApi.getOverview,
  })

  const { data: circulationStats } = useQuery({
    queryKey: ['circulation-stats'],
    queryFn: () => dashboardApi.getCirculationStats(),
  })

  const { data: categoryStats } = useQuery({
    queryKey: ['category-stats'],
    queryFn: dashboardApi.getCategoryStats,
  })

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-64">
        <LoadingSpinner size="lg" />
      </div>
    )
  }

  const stats = [
    {
      name: 'Total Books',
      value: overview?.totalBooks || 0,
      icon: BookOpenIcon,
      color: 'text-blue-600',
      bgColor: 'bg-blue-100',
    },
    {
      name: 'Active Members',
      value: overview?.activeMembers || 0,
      icon: UsersIcon,
      color: 'text-green-600',
      bgColor: 'bg-green-100',
    },
    {
      name: 'Active Transactions',
      value: overview?.totalTransactions || 0,
      icon: ArrowsRightLeftIcon,
      color: 'text-purple-600',
      bgColor: 'bg-purple-100',
    },
    {
      name: 'Overdue Books',
      value: overview?.overdueBooks || 0,
      icon: ExclamationTriangleIcon,
      color: 'text-red-600',
      bgColor: 'bg-red-100',
    },
    {
      name: 'Total Fines',
      value: `$${overview?.totalFines || 0}`,
      icon: CurrencyDollarIcon,
      color: 'text-yellow-600',
      bgColor: 'bg-yellow-100',
    },
    {
      name: 'Active Reservations',
      value: overview?.activeReservations || 0,
      icon: CalendarDaysIcon,
      color: 'text-indigo-600',
      bgColor: 'bg-indigo-100',
    },
  ]

  const mockChartData = [
    { name: 'Jan', checkouts: 120, returns: 110 },
    { name: 'Feb', checkouts: 150, returns: 140 },
    { name: 'Mar', checkouts: 180, returns: 170 },
    { name: 'Apr', checkouts: 200, returns: 190 },
    { name: 'May', checkouts: 170, returns: 180 },
    { name: 'Jun', checkouts: 190, returns: 185 },
  ]

  const categoryData = [
    { name: 'Fiction', value: 400, color: '#3B82F6' },
    { name: 'Science', value: 300, color: '#10B981' },
    { name: 'History', value: 200, color: '#F59E0B' },
    { name: 'Technology', value: 150, color: '#EF4444' },
    { name: 'Arts', value: 100, color: '#8B5CF6' },
  ]

  return (
    <div className="space-y-8">
      <div>
        <h1 className="text-2xl font-bold text-gray-900">Dashboard</h1>
        <p className="mt-1 text-sm text-gray-500">
          Welcome back! Here's what's happening in your library today.
        </p>
      </div>

      {/* Stats Grid */}
      <div className="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-3">
        {stats.map((stat) => (
          <Card key={stat.name} className="hover:shadow-md transition-shadow">
            <CardContent className="p-6">
              <div className="flex items-center">
                <div className={`p-3 rounded-lg ${stat.bgColor}`}>
                  <stat.icon className={`h-6 w-6 ${stat.color}`} />
                </div>
                <div className="ml-4">
                  <p className="text-sm font-medium text-gray-600">{stat.name}</p>
                  <p className="text-2xl font-bold text-gray-900">{stat.value}</p>
                </div>
              </div>
            </CardContent>
          </Card>
        ))}
      </div>

      {/* Charts Grid */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Circulation Chart */}
        <Card>
          <CardHeader>
            <CardTitle>Monthly Circulation</CardTitle>
          </CardHeader>
          <CardContent>
            <ResponsiveContainer width="100%" height={300}>
              <BarChart data={mockChartData}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="name" />
                <YAxis />
                <Tooltip />
                <Bar dataKey="checkouts" fill="#3B82F6" name="Checkouts" />
                <Bar dataKey="returns" fill="#10B981" name="Returns" />
              </BarChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>

        {/* Category Distribution */}
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
      </div>

      {/* Recent Activities */}
      <Card>
        <CardHeader>
          <CardTitle>Recent Activities</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="space-y-4">
            {overview?.recentActivities?.map((activity) => (
              <div key={activity.id} className="flex items-center space-x-4 p-4 bg-gray-50 rounded-lg">
                <div className="flex-1">
                  <p className="text-sm font-medium text-gray-900">{activity.description}</p>
                  <p className="text-xs text-gray-500">
                    {activity.userName} • {new Date(activity.timestamp).toLocaleString()}
                  </p>
                </div>
                <div className="text-xs text-gray-500">{activity.status}</div>
              </div>
            )) || (
              <p className="text-gray-500 text-center py-8">No recent activities</p>
            )}
          </div>
        </CardContent>
      </Card>
    </div>
  )
}