import { useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { notificationsApi } from '@/services/api'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card'
import Button from '@/components/ui/Button'
import Badge from '@/components/ui/Badge'
import LoadingSpinner from '@/components/ui/LoadingSpinner'
import { NotificationStatus, NotificationType } from '@/types'
import { BellIcon, CheckIcon, XMarkIcon } from '@heroicons/react/24/outline'

export default function NotificationsPage() {
  const [selectedNotifications, setSelectedNotifications] = useState<number[]>([])

  // Mock data since we don't have a current user context
  const mockMemberId = 1

  const { data: notifications, isLoading, refetch } = useQuery({
    queryKey: ['notifications', mockMemberId],
    queryFn: () => notificationsApi.getByMember(mockMemberId, { pageSize: 50 }),
  })

  const getTypeBadge = (type: NotificationType) => {
    const variants = {
      [NotificationType.BookDue]: 'warning',
      [NotificationType.BookOverdue]: 'error',
      [NotificationType.BookAvailable]: 'success',
      [NotificationType.ReservationExpiring]: 'warning',
      [NotificationType.FineIssued]: 'error',
      [NotificationType.AccountSuspended]: 'error',
      [NotificationType.General]: 'info',
    } as const

    return <Badge variant={variants[type]}>{type}</Badge>
  }

  const getStatusBadge = (status: NotificationStatus) => {
    const variants = {
      [NotificationStatus.Unread]: 'warning',
      [NotificationStatus.Read]: 'success',
      [NotificationStatus.Archived]: 'default',
    } as const

    return <Badge variant={variants[status]}>{status}</Badge>
  }

  const handleMarkAsRead = async (id: number) => {
    try {
      await notificationsApi.markAsRead(id)
      refetch()
    } catch (error) {
      console.error('Failed to mark as read:', error)
    }
  }

  const handleMarkMultipleAsRead = async () => {
    if (selectedNotifications.length === 0) return
    
    try {
      await notificationsApi.markMultipleAsRead(selectedNotifications)
      setSelectedNotifications([])
      refetch()
    } catch (error) {
      console.error('Failed to mark multiple as read:', error)
    }
  }

  const handleSelectNotification = (id: number) => {
    setSelectedNotifications(prev => 
      prev.includes(id) 
        ? prev.filter(nId => nId !== id)
        : [...prev, id]
    )
  }

  const handleSelectAll = () => {
    if (selectedNotifications.length === notifications?.items.length) {
      setSelectedNotifications([])
    } else {
      setSelectedNotifications(notifications?.items.map(n => n.id) || [])
    }
  }

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-64">
        <LoadingSpinner size="lg" />
      </div>
    )
  }

  const unreadCount = notifications?.items.filter(n => n.status === NotificationStatus.Unread).length || 0

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Notifications</h1>
          <p className="mt-1 text-sm text-gray-500">
            Stay updated with library activities and alerts
          </p>
        </div>
        <div className="flex items-center space-x-2">
          {unreadCount > 0 && (
            <Badge variant="error">{unreadCount} unread</Badge>
          )}
          {selectedNotifications.length > 0 && (
            <Button
              variant="outline"
              size="sm"
              onClick={handleMarkMultipleAsRead}
            >
              <CheckIcon className="h-4 w-4 mr-1" />
              Mark Selected as Read
            </Button>
          )}
        </div>
      </div>

      <Card>
        <CardHeader>
          <div className="flex justify-between items-center">
            <CardTitle>All Notifications ({notifications?.items.length || 0})</CardTitle>
            <div className="flex items-center space-x-2">
              <input
                type="checkbox"
                checked={selectedNotifications.length === notifications?.items.length && notifications?.items.length > 0}
                onChange={handleSelectAll}
                className="h-4 w-4 rounded border-gray-300 text-primary-600 focus:ring-primary-600"
              />
              <label className="text-sm text-gray-600">Select All</label>
            </div>
          </div>
        </CardHeader>
        <CardContent className="p-0">
          <div className="divide-y divide-gray-200">
            {notifications?.items.map((notification) => (
              <div
                key={notification.id}
                className={`p-6 hover:bg-gray-50 transition-colors ${
                  notification.status === NotificationStatus.Unread ? 'bg-blue-50' : ''
                }`}
              >
                <div className="flex items-start space-x-4">
                  <input
                    type="checkbox"
                    checked={selectedNotifications.includes(notification.id)}
                    onChange={() => handleSelectNotification(notification.id)}
                    className="mt-1 h-4 w-4 rounded border-gray-300 text-primary-600 focus:ring-primary-600"
                  />
                  
                  <div className="flex-shrink-0">
                    <div className={`p-2 rounded-full ${
                      notification.status === NotificationStatus.Unread ? 'bg-primary-100' : 'bg-gray-100'
                    }`}>
                      <BellIcon className={`h-5 w-5 ${
                        notification.status === NotificationStatus.Unread ? 'text-primary-600' : 'text-gray-400'
                      }`} />
                    </div>
                  </div>

                  <div className="flex-1 min-w-0">
                    <div className="flex items-center justify-between">
                      <h3 className={`text-sm font-medium ${
                        notification.status === NotificationStatus.Unread ? 'text-gray-900' : 'text-gray-600'
                      }`}>
                        {notification.title}
                      </h3>
                      <div className="flex items-center space-x-2">
                        {getTypeBadge(notification.type)}
                        {getStatusBadge(notification.status)}
                      </div>
                    </div>
                    
                    <p className={`mt-1 text-sm ${
                      notification.status === NotificationStatus.Unread ? 'text-gray-700' : 'text-gray-500'
                    }`}>
                      {notification.message}
                    </p>
                    
                    <div className="mt-2 flex items-center justify-between">
                      <p className="text-xs text-gray-500">
                        {new Date(notification.createdAt).toLocaleString()}
                      </p>
                      
                      <div className="flex items-center space-x-2">
                        {notification.status === NotificationStatus.Unread && (
                          <Button
                            size="sm"
                            variant="outline"
                            onClick={() => handleMarkAsRead(notification.id)}
                          >
                            <CheckIcon className="h-4 w-4 mr-1" />
                            Mark as Read
                          </Button>
                        )}
                        <Button
                          size="sm"
                          variant="ghost"
                          onClick={() => notificationsApi.delete(notification.id)}
                        >
                          <XMarkIcon className="h-4 w-4" />
                        </Button>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            ))}
            
            {notifications?.items.length === 0 && (
              <div className="text-center py-12">
                <BellIcon className="h-12 w-12 text-gray-400 mx-auto mb-4" />
                <h3 className="text-lg font-medium text-gray-900">No notifications</h3>
                <p className="text-gray-500">You're all caught up!</p>
              </div>
            )}
          </div>
        </CardContent>
      </Card>
    </div>
  )
}