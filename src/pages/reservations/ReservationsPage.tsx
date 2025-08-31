import { useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { reservationsApi } from '@/services/api'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/Table'
import Button from '@/components/ui/Button'
import Badge from '@/components/ui/Badge'
import Pagination from '@/components/ui/Pagination'
import LoadingSpinner from '@/components/ui/LoadingSpinner'
import { ReservationStatus } from '@/types'
import { Link } from 'react-router-dom'

export default function ReservationsPage() {
  const [page, setPage] = useState(1)
  const [pageSize] = useState(20)
  const [activeTab, setActiveTab] = useState<'active' | 'expired'>('active')

  const { data: reservationsData, isLoading } = useQuery({
    queryKey: ['reservations', activeTab, page, pageSize],
    queryFn: () => {
      switch (activeTab) {
        case 'active':
          return reservationsApi.getActive({ page, pageSize })
        case 'expired':
          return reservationsApi.getExpired({ page, pageSize })
        default:
          return reservationsApi.getActive({ page, pageSize })
      }
    },
  })

  const getStatusBadge = (status: ReservationStatus) => {
    const variants = {
      [ReservationStatus.Active]: 'info',
      [ReservationStatus.Fulfilled]: 'success',
      [ReservationStatus.Expired]: 'error',
      [ReservationStatus.Cancelled]: 'default',
      [ReservationStatus.OnHold]: 'warning',
    } as const

    return <Badge variant={variants[status]}>{status}</Badge>
  }

  const tabs = [
    { key: 'active', label: 'Active Reservations' },
    { key: 'expired', label: 'Expired Reservations' },
  ]

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
          <h1 className="text-2xl font-bold text-gray-900">Reservations</h1>
          <p className="mt-1 text-sm text-gray-500">
            Manage book reservations and waiting lists
          </p>
        </div>
        <Button>
          New Reservation
        </Button>
      </div>

      {/* Tabs */}
      <div className="border-b border-gray-200">
        <nav className="-mb-px flex space-x-8">
          {tabs.map((tab) => (
            <button
              key={tab.key}
              onClick={() => setActiveTab(tab.key as any)}
              className={`py-2 px-1 border-b-2 font-medium text-sm ${
                activeTab === tab.key
                  ? 'border-primary-500 text-primary-600'
                  : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
              }`}
            >
              {tab.label}
            </button>
          ))}
        </nav>
      </div>

      {/* Reservations Table */}
      <Card>
        <CardHeader>
          <CardTitle>
            {activeTab === 'active' ? 'Active' : 'Expired'} Reservations ({reservationsData?.totalCount || 0})
          </CardTitle>
        </CardHeader>
        <CardContent className="p-0">
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Reservation #</TableHead>
                <TableHead>Book</TableHead>
                <TableHead>Member</TableHead>
                <TableHead>Reservation Date</TableHead>
                <TableHead>Expiry Date</TableHead>
                <TableHead>Priority</TableHead>
                <TableHead>Status</TableHead>
                <TableHead>Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {reservationsData?.items.map((reservation) => (
                <TableRow key={reservation.id}>
                  <TableCell className="font-mono text-sm">
                    {reservation.reservationNumber}
                  </TableCell>
                  <TableCell>
                    <Link
                      to={`/books/${reservation.bookId}`}
                      className="font-medium text-gray-900 hover:text-primary-600"
                    >
                      {reservation.bookTitle}
                    </Link>
                  </TableCell>
                  <TableCell>
                    <Link
                      to={`/members/${reservation.memberId}`}
                      className="font-medium text-gray-900 hover:text-primary-600"
                    >
                      {reservation.memberName}
                    </Link>
                  </TableCell>
                  <TableCell>
                    {new Date(reservation.reservationDate).toLocaleDateString()}
                  </TableCell>
                  <TableCell>
                    <span className={new Date(reservation.expiryDate) < new Date() ? 'text-red-600 font-medium' : ''}>
                      {new Date(reservation.expiryDate).toLocaleDateString()}
                    </span>
                  </TableCell>
                  <TableCell>
                    <Badge variant={reservation.priority <= 2 ? 'error' : reservation.priority <= 5 ? 'warning' : 'default'}>
                      {reservation.priority}
                    </Badge>
                  </TableCell>
                  <TableCell>{getStatusBadge(reservation.status)}</TableCell>
                  <TableCell>
                    <div className="flex space-x-2">
                      {reservation.status === ReservationStatus.Active && (
                        <>
                          <Button size="sm" variant="outline">
                            Fulfill
                          </Button>
                          <Button size="sm" variant="ghost">
                            Cancel
                          </Button>
                        </>
                      )}
                      <Button size="sm" variant="ghost">
                        View
                      </Button>
                    </div>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>

          {reservationsData && (
            <Pagination
              currentPage={page}
              totalPages={Math.ceil(reservationsData.totalCount / pageSize)}
              onPageChange={setPage}
              totalItems={reservationsData.totalCount}
              itemsPerPage={pageSize}
            />
          )}
        </CardContent>
      </Card>
    </div>
  )
}