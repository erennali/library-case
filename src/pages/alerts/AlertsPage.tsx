import { useQuery } from '@tanstack/react-query'
import { alertsApi } from '@/services/api'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card'
import Button from '@/components/ui/Button'
import Badge from '@/components/ui/Badge'
import LoadingSpinner from '@/components/ui/LoadingSpinner'
import { 
  ExclamationTriangleIcon, 
  InformationCircleIcon, 
  CheckCircleIcon,
  XCircleIcon,
  ClockIcon,
  XMarkIcon 
} from '@heroicons/react/24/outline'

export default function AlertsPage() {
  const { data: alertsSummary, isLoading } = useQuery({
    queryKey: ['alerts-summary'],
    queryFn: alertsApi.getSummary,
  })

  const { data: overdueAlerts } = useQuery({
    queryKey: ['overdue-alerts'],
    queryFn: alertsApi.getOverdueAlerts,
  })

  const { data: fineAlerts } = useQuery({
    queryKey: ['fine-alerts'],
    queryFn: alertsApi.getFineAlerts,
  })

  const { data: reservationAlerts } = useQuery({
    queryKey: ['reservation-alerts'],
    queryFn: alertsApi.getReservationAlerts,
  })

  const { data: membershipAlerts } = useQuery({
    queryKey: ['membership-alerts'],
    queryFn: alertsApi.getMembershipAlerts,
  })

  const handleDismissAlert = async (alertId: number) => {
    try {
      await alertsApi.dismissAlert(alertId)
      // Refetch all queries
    } catch (error) {
      console.error('Failed to dismiss alert:', error)
    }
  }

  const handleDismissAll = async () => {
    try {
      await alertsApi.dismissAllAlerts()
      // Refetch all queries
    } catch (error) {
      console.error('Failed to dismiss all alerts:', error)
    }
  }

  const getSeverityIcon = (severity: string) => {
    switch (severity.toLowerCase()) {
      case 'critical':
        return <XCircleIcon className="h-5 w-5 text-red-600" />
      case 'high':
        return <ExclamationTriangleIcon className="h-5 w-5 text-orange-600" />
      case 'medium':
        return <InformationCircleIcon className="h-5 w-5 text-blue-600" />
      case 'low':
        return <CheckCircleIcon className="h-5 w-5 text-green-600" />
      default:
        return <ClockIcon className="h-5 w-5 text-gray-600" />
    }
  }

  const getSeverityBadge = (severity: string) => {
    const variants: Record<string, 'success' | 'warning' | 'error' | 'info' | 'default'> = {
      'critical': 'error',
      'high': 'warning',
      'medium': 'info',
      'low': 'success',
    }

    return <Badge variant={variants[severity.toLowerCase()] || 'default'}>{severity}</Badge>
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
          <h1 className="text-2xl font-bold text-gray-900">Alerts</h1>
          <p className="mt-1 text-sm text-gray-500">
            Monitor important library events and notifications
          </p>
        </div>
        {alertsSummary && alertsSummary.totalAlerts > 0 && (
          <Button variant="outline" onClick={handleDismissAll}>
            Dismiss All Alerts
          </Button>
        )}
      </div>

      {/* Summary Cards */}
      {alertsSummary && (
        <div className="grid grid-cols-2 md:grid-cols-5 gap-4">
          <Card>
            <CardContent className="p-4 text-center">
              <p className="text-2xl font-bold text-gray-900">{alertsSummary.totalAlerts}</p>
              <p className="text-sm text-gray-600">Total Alerts</p>
            </CardContent>
          </Card>
          <Card>
            <CardContent className="p-4 text-center">
              <p className="text-2xl font-bold text-red-600">{alertsSummary.overdueAlerts}</p>
              <p className="text-sm text-gray-600">Overdue</p>
            </CardContent>
          </Card>
          <Card>
            <CardContent className="p-4 text-center">
              <p className="text-2xl font-bold text-yellow-600">{alertsSummary.fineAlerts}</p>
              <p className="text-sm text-gray-600">Fines</p>
            </CardContent>
          </Card>
          <Card>
            <CardContent className="p-4 text-center">
              <p className="text-2xl font-bold text-blue-600">{alertsSummary.reservationAlerts}</p>
              <p className="text-sm text-gray-600">Reservations</p>
            </CardContent>
          </Card>
          <Card>
            <CardContent className="p-4 text-center">
              <p className="text-2xl font-bold text-purple-600">{alertsSummary.membershipAlerts}</p>
              <p className="text-sm text-gray-600">Memberships</p>
            </CardContent>
          </Card>
        </div>
      )}

      {/* Overdue Alerts */}
      {overdueAlerts && overdueAlerts.overdueItems.length > 0 && (
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center">
              <ExclamationTriangleIcon className="h-5 w-5 text-red-600 mr-2" />
              Overdue Books ({overdueAlerts.totalOverdue})
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="space-y-3">
              {overdueAlerts.overdueItems.map((item, index) => (
                <div key={index} className="flex items-center justify-between p-4 bg-red-50 border border-red-200 rounded-lg">
                  <div className="flex-1">
                    <div className="flex items-center space-x-2">
                      <Link to={`/books/${item.bookId}`} className="font-medium text-gray-900 hover:text-primary-600">
                        {item.bookTitle}
                      </Link>
                      <Badge variant="error">{item.daysOverdue} days overdue</Badge>
                    </div>
                    <p className="text-sm text-gray-600">
                      Borrowed by{' '}
                      <Link to={`/members/${item.memberId}`} className="font-medium hover:text-primary-600">
                        {item.memberName}
                      </Link>
                    </p>
                    <p className="text-sm text-red-600 font-medium">
                      Fine: ${item.fineAmount.toFixed(2)}
                    </p>
                  </div>
                  <div className="flex space-x-2">
                    <Button size="sm" variant="outline">
                      Send Reminder
                    </Button>
                    <Button size="sm" variant="ghost">
                      <XMarkIcon className="h-4 w-4" />
                    </Button>
                  </div>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>
      )}

      {/* Fine Alerts */}
      {fineAlerts && fineAlerts.fineItems.length > 0 && (
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center">
              <CurrencyDollarIcon className="h-5 w-5 text-yellow-600 mr-2" />
              Outstanding Fines ({fineAlerts.totalFines})
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="space-y-3">
              {fineAlerts.fineItems.map((item, index) => (
                <div key={index} className="flex items-center justify-between p-4 bg-yellow-50 border border-yellow-200 rounded-lg">
                  <div className="flex-1">
                    <div className="flex items-center space-x-2">
                      <Link to={`/members/${item.memberId}`} className="font-medium text-gray-900 hover:text-primary-600">
                        {item.memberName}
                      </Link>
                      <Badge variant="warning">Fine #{item.fineId}</Badge>
                    </div>
                    <p className="text-sm text-gray-600">
                      Amount: ${item.amount.toFixed(2)}
                    </p>
                    <p className="text-sm text-gray-600">
                      Due: {new Date(item.dueDate).toLocaleDateString()}
                    </p>
                  </div>
                  <div className="flex space-x-2">
                    <Button size="sm" variant="outline">
                      Process Payment
                    </Button>
                    <Button size="sm" variant="ghost">
                      <XMarkIcon className="h-4 w-4" />
                    </Button>
                  </div>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>
      )}

      {/* Reservation Alerts */}
      {reservationAlerts && reservationAlerts.reservationItems.length > 0 && (
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center">
              <CalendarDaysIcon className="h-5 w-5 text-blue-600 mr-2" />
              Reservation Alerts ({reservationAlerts.totalReservations})
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="space-y-3">
              {reservationAlerts.reservationItems.map((item, index) => (
                <div key={index} className="flex items-center justify-between p-4 bg-blue-50 border border-blue-200 rounded-lg">
                  <div className="flex-1">
                    <div className="flex items-center space-x-2">
                      <Link to={`/books/${item.bookId}`} className="font-medium text-gray-900 hover:text-primary-600">
                        {item.bookTitle}
                      </Link>
                      <Badge variant={item.isExpired ? 'error' : 'warning'}>
                        {item.isExpired ? 'Expired' : 'Expiring Soon'}
                      </Badge>
                    </div>
                    <p className="text-sm text-gray-600">
                      Reserved by{' '}
                      <Link to={`/members/${item.memberId}`} className="font-medium hover:text-primary-600">
                        {item.memberName}
                      </Link>
                    </p>
                    <p className="text-sm text-gray-600">
                      Expires: {new Date(item.expiryDate).toLocaleDateString()}
                    </p>
                  </div>
                  <div className="flex space-x-2">
                    <Button size="sm" variant="outline">
                      {item.isExpired ? 'Cancel' : 'Fulfill'}
                    </Button>
                    <Button size="sm" variant="ghost">
                      <XMarkIcon className="h-4 w-4" />
                    </Button>
                  </div>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>
      )}

      {/* Membership Alerts */}
      {membershipAlerts && membershipAlerts.membershipItems.length > 0 && (
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center">
              <UserIcon className="h-5 w-5 text-purple-600 mr-2" />
              Membership Alerts ({membershipAlerts.expiringMemberships + membershipAlerts.expiredMemberships})
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="space-y-3">
              {membershipAlerts.membershipItems.map((item, index) => (
                <div key={index} className="flex items-center justify-between p-4 bg-purple-50 border border-purple-200 rounded-lg">
                  <div className="flex-1">
                    <div className="flex items-center space-x-2">
                      <Link to={`/members/${item.memberId}`} className="font-medium text-gray-900 hover:text-primary-600">
                        {item.memberName}
                      </Link>
                      <Badge variant={item.isExpired ? 'error' : 'warning'}>
                        {item.isExpired ? 'Expired' : `${item.daysUntilExpiry} days left`}
                      </Badge>
                    </div>
                    <p className="text-sm text-gray-600">
                      Membership ends: {new Date(item.membershipEndDate).toLocaleDateString()}
                    </p>
                  </div>
                  <div className="flex space-x-2">
                    <Button size="sm" variant="outline">
                      Extend Membership
                    </Button>
                    <Button size="sm" variant="ghost">
                      <XMarkIcon className="h-4 w-4" />
                    </Button>
                  </div>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>
      )}

      {/* No Alerts State */}
      {alertsSummary && alertsSummary.totalAlerts === 0 && (
        <Card>
          <CardContent className="p-12 text-center">
            <CheckCircleIcon className="h-12 w-12 text-green-600 mx-auto mb-4" />
            <h3 className="text-lg font-medium text-gray-900">All Clear!</h3>
            <p className="text-gray-500">No alerts require your attention at this time.</p>
          </CardContent>
        </Card>
      )}
    </div>
  )
}