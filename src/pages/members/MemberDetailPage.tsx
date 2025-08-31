import { useParams, Link } from 'react-router-dom'
import { useQuery } from '@tanstack/react-query'
import { membersApi, transactionsApi, finesApi, reservationsApi } from '@/services/api'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/Table'
import Button from '@/components/ui/Button'
import Badge from '@/components/ui/Badge'
import LoadingSpinner from '@/components/ui/LoadingSpinner'
import { ArrowLeftIcon, UserIcon, CurrencyDollarIcon, BookOpenIcon, CalendarDaysIcon } from '@heroicons/react/24/outline'
import { MemberStatus, MembershipType, TransactionStatus, FineStatus, ReservationStatus } from '@/types'

export default function MemberDetailPage() {
  const { id } = useParams<{ id: string }>()
  const memberId = Number(id)

  const { data: member, isLoading } = useQuery({
    queryKey: ['member', memberId],
    queryFn: () => membersApi.getById(memberId),
    enabled: !!memberId,
  })

  const { data: transactions } = useQuery({
    queryKey: ['member-transactions', memberId],
    queryFn: () => transactionsApi.getByMember(memberId, { pageSize: 10 }),
    enabled: !!memberId,
  })

  const { data: fines } = useQuery({
    queryKey: ['member-fines', memberId],
    queryFn: () => finesApi.getByMember(memberId, { pageSize: 10 }),
    enabled: !!memberId,
  })

  const { data: reservations } = useQuery({
    queryKey: ['member-reservations', memberId],
    queryFn: () => reservationsApi.getByMember(memberId, { pageSize: 10 }),
    enabled: !!memberId,
  })

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-64">
        <LoadingSpinner size="lg" />
      </div>
    )
  }

  if (!member) {
    return (
      <div className="text-center py-12">
        <h2 className="text-2xl font-bold text-gray-900">Member not found</h2>
        <p className="mt-2 text-gray-600">The member you're looking for doesn't exist.</p>
        <Link to="/members">
          <Button className="mt-4">
            <ArrowLeftIcon className="h-4 w-4 mr-2" />
            Back to Members
          </Button>
        </Link>
      </div>
    )
  }

  const getStatusBadge = (status: MemberStatus) => {
    const variants = {
      [MemberStatus.Active]: 'success',
      [MemberStatus.Suspended]: 'warning',
      [MemberStatus.Expired]: 'error',
      [MemberStatus.Blocked]: 'error',
    } as const

    return <Badge variant={variants[status]}>{status}</Badge>
  }

  const getMembershipTypeBadge = (type: MembershipType) => {
    const variants = {
      [MembershipType.Student]: 'info',
      [MembershipType.Faculty]: 'success',
      [MembershipType.Staff]: 'warning',
      [MembershipType.Regular]: 'default',
      [MembershipType.Premium]: 'info',
    } as const

    return <Badge variant={variants[type]}>{type}</Badge>
  }

  const getTransactionStatusBadge = (status: TransactionStatus) => {
    const variants = {
      [TransactionStatus.Active]: 'info',
      [TransactionStatus.Borrowed]: 'info',
      [TransactionStatus.Returned]: 'success',
      [TransactionStatus.Overdue]: 'error',
      [TransactionStatus.Lost]: 'error',
      [TransactionStatus.Damaged]: 'warning',
    } as const

    return <Badge variant={variants[status]}>{status}</Badge>
  }

  const getFineStatusBadge = (status: FineStatus) => {
    const variants = {
      [FineStatus.Pending]: 'warning',
      [FineStatus.Unpaid]: 'error',
      [FineStatus.Overdue]: 'error',
      [FineStatus.PartiallyPaid]: 'warning',
      [FineStatus.Paid]: 'success',
      [FineStatus.Waived]: 'info',
      [FineStatus.Cancelled]: 'default',
    } as const

    return <Badge variant={variants[status]}>{status}</Badge>
  }

  const getReservationStatusBadge = (status: ReservationStatus) => {
    const variants = {
      [ReservationStatus.Active]: 'info',
      [ReservationStatus.Fulfilled]: 'success',
      [ReservationStatus.Expired]: 'error',
      [ReservationStatus.Cancelled]: 'default',
      [ReservationStatus.OnHold]: 'warning',
    } as const

    return <Badge variant={variants[status]}>{status}</Badge>
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div className="flex items-center space-x-4">
          <Link to="/members">
            <Button variant="outline" size="sm">
              <ArrowLeftIcon className="h-4 w-4 mr-2" />
              Back
            </Button>
          </Link>
          <div>
            <h1 className="text-2xl font-bold text-gray-900">
              {member.firstName} {member.lastName}
            </h1>
            <p className="text-gray-600">#{member.membershipNumber}</p>
          </div>
        </div>
        <div className="flex space-x-2">
          <Button variant="outline">Edit Member</Button>
          <Button variant="outline">Extend Membership</Button>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-4 gap-6">
        {/* Stats Cards */}
        <Card>
          <CardContent className="p-6">
            <div className="flex items-center">
              <div className="p-3 rounded-lg bg-blue-100">
                <BookOpenIcon className="h-6 w-6 text-blue-600" />
              </div>
              <div className="ml-4">
                <p className="text-sm font-medium text-gray-600">Current Books</p>
                <p className="text-2xl font-bold text-gray-900">
                  {member.currentBooksCount}/{member.maxBooksAllowed}
                </p>
              </div>
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardContent className="p-6">
            <div className="flex items-center">
              <div className="p-3 rounded-lg bg-red-100">
                <CurrencyDollarIcon className="h-6 w-6 text-red-600" />
              </div>
              <div className="ml-4">
                <p className="text-sm font-medium text-gray-600">Total Fines</p>
                <p className="text-2xl font-bold text-gray-900">
                  ${member.totalFinesOwed.toFixed(2)}
                </p>
              </div>
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardContent className="p-6">
            <div className="flex items-center">
              <div className="p-3 rounded-lg bg-green-100">
                <CalendarDaysIcon className="h-6 w-6 text-green-600" />
              </div>
              <div className="ml-4">
                <p className="text-sm font-medium text-gray-600">Reservations</p>
                <p className="text-2xl font-bold text-gray-900">
                  {reservations?.items.filter(r => r.status === ReservationStatus.Active).length || 0}
                </p>
              </div>
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardContent className="p-6">
            <div className="flex items-center">
              <div className="p-3 rounded-lg bg-purple-100">
                <UserIcon className="h-6 w-6 text-purple-600" />
              </div>
              <div className="ml-4">
                <p className="text-sm font-medium text-gray-600">Status</p>
                <div className="mt-1">{getStatusBadge(member.status)}</div>
              </div>
            </div>
          </CardContent>
        </Card>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        {/* Member Details */}
        <div className="lg:col-span-2 space-y-6">
          <Card>
            <CardHeader>
              <CardTitle>Member Information</CardTitle>
            </CardHeader>
            <CardContent>
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="text-sm font-medium text-gray-500">Email</label>
                  <p className="text-sm text-gray-900">{member.email}</p>
                </div>
                <div>
                  <label className="text-sm font-medium text-gray-500">Phone</label>
                  <p className="text-sm text-gray-900">{member.phoneNumber || 'N/A'}</p>
                </div>
                <div>
                  <label className="text-sm font-medium text-gray-500">Date of Birth</label>
                  <p className="text-sm text-gray-900">
                    {new Date(member.dateOfBirth).toLocaleDateString()}
                  </p>
                </div>
                <div>
                  <label className="text-sm font-medium text-gray-500">Membership Type</label>
                  <div>{getMembershipTypeBadge(member.membershipType)}</div>
                </div>
                <div>
                  <label className="text-sm font-medium text-gray-500">Membership Start</label>
                  <p className="text-sm text-gray-900">
                    {new Date(member.membershipStartDate).toLocaleDateString()}
                  </p>
                </div>
                <div>
                  <label className="text-sm font-medium text-gray-500">Membership End</label>
                  <p className="text-sm text-gray-900">
                    {new Date(member.membershipEndDate).toLocaleDateString()}
                  </p>
                </div>
              </div>
              {member.address && (
                <div className="mt-4">
                  <label className="text-sm font-medium text-gray-500">Address</label>
                  <p className="text-sm text-gray-900 mt-1">{member.address}</p>
                </div>
              )}
            </CardContent>
          </Card>

          {/* Current Transactions */}
          <Card>
            <CardHeader>
              <CardTitle>Current Transactions</CardTitle>
            </CardHeader>
            <CardContent className="p-0">
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead>Book</TableHead>
                    <TableHead>Checkout Date</TableHead>
                    <TableHead>Due Date</TableHead>
                    <TableHead>Status</TableHead>
                    <TableHead>Actions</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {transactions?.items
                    .filter(t => t.status === TransactionStatus.Active || t.status === TransactionStatus.Borrowed)
                    .map((transaction) => (
                    <TableRow key={transaction.id}>
                      <TableCell>
                        <Link
                          to={`/books/${transaction.bookId}`}
                          className="font-medium text-gray-900 hover:text-primary-600"
                        >
                          {transaction.bookTitle}
                        </Link>
                      </TableCell>
                      <TableCell>
                        {new Date(transaction.checkoutDate).toLocaleDateString()}
                      </TableCell>
                      <TableCell>
                        <span className={new Date(transaction.dueDate) < new Date() ? 'text-red-600 font-medium' : ''}>
                          {new Date(transaction.dueDate).toLocaleDateString()}
                        </span>
                      </TableCell>
                      <TableCell>{getTransactionStatusBadge(transaction.status)}</TableCell>
                      <TableCell>
                        <div className="flex space-x-2">
                          <Button size="sm" variant="outline">
                            Return
                          </Button>
                          <Button size="sm" variant="ghost">
                            Renew
                          </Button>
                        </div>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </CardContent>
          </Card>

          {/* Outstanding Fines */}
          <Card>
            <CardHeader>
              <CardTitle>Outstanding Fines</CardTitle>
            </CardHeader>
            <CardContent className="p-0">
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead>Fine #</TableHead>
                    <TableHead>Type</TableHead>
                    <TableHead>Amount</TableHead>
                    <TableHead>Issue Date</TableHead>
                    <TableHead>Status</TableHead>
                    <TableHead>Actions</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {fines?.items
                    .filter(f => f.status !== FineStatus.Paid && f.status !== FineStatus.Waived)
                    .map((fine) => (
                    <TableRow key={fine.id}>
                      <TableCell>{fine.fineNumber}</TableCell>
                      <TableCell>{fine.type}</TableCell>
                      <TableCell>${fine.amount.toFixed(2)}</TableCell>
                      <TableCell>
                        {new Date(fine.issueDate).toLocaleDateString()}
                      </TableCell>
                      <TableCell>{getFineStatusBadge(fine.status)}</TableCell>
                      <TableCell>
                        <div className="flex space-x-2">
                          <Button size="sm" variant="outline">
                            Pay
                          </Button>
                          <Button size="sm" variant="ghost">
                            Waive
                          </Button>
                        </div>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </CardContent>
          </Card>
        </div>

        {/* Sidebar */}
        <div className="space-y-6">
          {/* Member Avatar */}
          <Card>
            <CardContent className="p-6 text-center">
              <div className="h-24 w-24 rounded-full bg-primary-600 flex items-center justify-center mx-auto mb-4">
                <span className="text-2xl font-bold text-white">
                  {member.firstName[0]}{member.lastName[0]}
                </span>
              </div>
              <h3 className="text-lg font-medium text-gray-900">
                {member.firstName} {member.lastName}
              </h3>
              <p className="text-sm text-gray-500">{member.email}</p>
              <div className="mt-2">{getMembershipTypeBadge(member.membershipType)}</div>
            </CardContent>
          </Card>

          {/* Quick Stats */}
          <Card>
            <CardHeader>
              <CardTitle>Quick Stats</CardTitle>
            </CardHeader>
            <CardContent>
              <div className="space-y-3">
                <div className="flex justify-between">
                  <span className="text-sm text-gray-600">Books Borrowed</span>
                  <span className="text-sm font-medium">{member.currentBooksCount}</span>
                </div>
                <div className="flex justify-between">
                  <span className="text-sm text-gray-600">Total Fines</span>
                  <span className="text-sm font-medium text-red-600">
                    ${member.totalFinesOwed.toFixed(2)}
                  </span>
                </div>
                <div className="flex justify-between">
                  <span className="text-sm text-gray-600">Active Reservations</span>
                  <span className="text-sm font-medium">
                    {reservations?.items.filter(r => r.status === ReservationStatus.Active).length || 0}
                  </span>
                </div>
                <div className="flex justify-between">
                  <span className="text-sm text-gray-600">Member Since</span>
                  <span className="text-sm font-medium">
                    {new Date(member.membershipStartDate).toLocaleDateString()}
                  </span>
                </div>
              </div>
            </CardContent>
          </Card>

          {/* Quick Actions */}
          <Card>
            <CardHeader>
              <CardTitle>Quick Actions</CardTitle>
            </CardHeader>
            <CardContent>
              <div className="space-y-2">
                <Button variant="outline" className="w-full">
                  Borrow Book
                </Button>
                <Button variant="outline" className="w-full">
                  Pay Fine
                </Button>
                <Button variant="outline" className="w-full">
                  Send Notification
                </Button>
                <Button variant="outline" className="w-full">
                  View Full History
                </Button>
              </div>
            </CardContent>
          </Card>
        </div>
      </div>
    </div>
  )
}