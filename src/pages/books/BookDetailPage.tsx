import { useParams, Link } from 'react-router-dom'
import { useQuery } from '@tanstack/react-query'
import { booksApi, transactionsApi, reservationsApi, reviewsApi } from '@/services/api'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/Table'
import Button from '@/components/ui/Button'
import Badge from '@/components/ui/Badge'
import LoadingSpinner from '@/components/ui/LoadingSpinner'
import { ArrowLeftIcon, StarIcon } from '@heroicons/react/24/outline'
import { BookStatus, TransactionStatus, ReservationStatus } from '@/types'

export default function BookDetailPage() {
  const { id } = useParams<{ id: string }>()
  const bookId = Number(id)

  const { data: book, isLoading } = useQuery({
    queryKey: ['book', bookId],
    queryFn: () => booksApi.getById(bookId),
    enabled: !!bookId,
  })

  const { data: transactions } = useQuery({
    queryKey: ['book-transactions', bookId],
    queryFn: () => transactionsApi.getByBook(bookId, { pageSize: 10 }),
    enabled: !!bookId,
  })

  const { data: reservations } = useQuery({
    queryKey: ['book-reservations', bookId],
    queryFn: () => reservationsApi.getByBook(bookId, { pageSize: 10 }),
    enabled: !!bookId,
  })

  const { data: reviews } = useQuery({
    queryKey: ['book-reviews', bookId],
    queryFn: () => reviewsApi.getByBook(bookId, { pageSize: 10 }),
    enabled: !!bookId,
  })

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-64">
        <LoadingSpinner size="lg" />
      </div>
    )
  }

  if (!book) {
    return (
      <div className="text-center py-12">
        <h2 className="text-2xl font-bold text-gray-900">Book not found</h2>
        <p className="mt-2 text-gray-600">The book you're looking for doesn't exist.</p>
        <Link to="/books">
          <Button className="mt-4">
            <ArrowLeftIcon className="h-4 w-4 mr-2" />
            Back to Books
          </Button>
        </Link>
      </div>
    )
  }

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

  const averageRating = reviews?.items.length 
    ? reviews.items.reduce((sum, review) => sum + review.rating, 0) / reviews.items.length 
    : 0

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div className="flex items-center space-x-4">
          <Link to="/books">
            <Button variant="outline" size="sm">
              <ArrowLeftIcon className="h-4 w-4 mr-2" />
              Back
            </Button>
          </Link>
          <div>
            <h1 className="text-2xl font-bold text-gray-900">{book.title}</h1>
            <p className="text-gray-600">by {book.author}</p>
          </div>
        </div>
        <div className="flex space-x-2">
          <Button variant="outline">Edit Book</Button>
          <Button variant="error">Delete Book</Button>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        {/* Book Details */}
        <div className="lg:col-span-2 space-y-6">
          <Card>
            <CardHeader>
              <CardTitle>Book Information</CardTitle>
            </CardHeader>
            <CardContent>
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="text-sm font-medium text-gray-500">ISBN</label>
                  <p className="text-sm text-gray-900">{book.isbn}</p>
                </div>
                <div>
                  <label className="text-sm font-medium text-gray-500">Publisher</label>
                  <p className="text-sm text-gray-900">{book.publisher || 'N/A'}</p>
                </div>
                <div>
                  <label className="text-sm font-medium text-gray-500">Publication Date</label>
                  <p className="text-sm text-gray-900">
                    {book.publicationDate ? new Date(book.publicationDate).toLocaleDateString() : 'N/A'}
                  </p>
                </div>
                <div>
                  <label className="text-sm font-medium text-gray-500">Language</label>
                  <p className="text-sm text-gray-900">{book.language || 'N/A'}</p>
                </div>
                <div>
                  <label className="text-sm font-medium text-gray-500">Pages</label>
                  <p className="text-sm text-gray-900">{book.pageCount}</p>
                </div>
                <div>
                  <label className="text-sm font-medium text-gray-500">Price</label>
                  <p className="text-sm text-gray-900">${book.price || 'N/A'}</p>
                </div>
                <div>
                  <label className="text-sm font-medium text-gray-500">Status</label>
                  <div>{getStatusBadge(book.status)}</div>
                </div>
                <div>
                  <label className="text-sm font-medium text-gray-500">Copies</label>
                  <p className="text-sm text-gray-900">
                    {book.availableCopies} available / {book.totalCopies} total
                  </p>
                </div>
              </div>
              {book.description && (
                <div className="mt-4">
                  <label className="text-sm font-medium text-gray-500">Description</label>
                  <p className="text-sm text-gray-900 mt-1">{book.description}</p>
                </div>
              )}
            </CardContent>
          </Card>

          {/* Recent Transactions */}
          <Card>
            <CardHeader>
              <CardTitle>Recent Transactions</CardTitle>
            </CardHeader>
            <CardContent className="p-0">
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead>Member</TableHead>
                    <TableHead>Type</TableHead>
                    <TableHead>Date</TableHead>
                    <TableHead>Due Date</TableHead>
                    <TableHead>Status</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {transactions?.items.map((transaction) => (
                    <TableRow key={transaction.id}>
                      <TableCell>{transaction.memberName}</TableCell>
                      <TableCell>{transaction.type}</TableCell>
                      <TableCell>
                        {new Date(transaction.checkoutDate).toLocaleDateString()}
                      </TableCell>
                      <TableCell>
                        {new Date(transaction.dueDate).toLocaleDateString()}
                      </TableCell>
                      <TableCell>{getTransactionStatusBadge(transaction.status)}</TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </CardContent>
          </Card>

          {/* Active Reservations */}
          <Card>
            <CardHeader>
              <CardTitle>Active Reservations</CardTitle>
            </CardHeader>
            <CardContent className="p-0">
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead>Member</TableHead>
                    <TableHead>Reservation Date</TableHead>
                    <TableHead>Expiry Date</TableHead>
                    <TableHead>Priority</TableHead>
                    <TableHead>Status</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {reservations?.items.map((reservation) => (
                    <TableRow key={reservation.id}>
                      <TableCell>{reservation.memberName}</TableCell>
                      <TableCell>
                        {new Date(reservation.reservationDate).toLocaleDateString()}
                      </TableCell>
                      <TableCell>
                        {new Date(reservation.expiryDate).toLocaleDateString()}
                      </TableCell>
                      <TableCell>{reservation.priority}</TableCell>
                      <TableCell>{getReservationStatusBadge(reservation.status)}</TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </CardContent>
          </Card>
        </div>

        {/* Sidebar */}
        <div className="space-y-6">
          {/* Book Cover */}
          <Card>
            <CardContent className="p-6">
              {book.imageUrl ? (
                <img
                  src={book.imageUrl}
                  alt={book.title}
                  className="w-full h-64 object-cover rounded-lg"
                />
              ) : (
                <div className="w-full h-64 bg-gray-200 rounded-lg flex items-center justify-center">
                  <span className="text-gray-500">No image available</span>
                </div>
              )}
            </CardContent>
          </Card>

          {/* Rating & Reviews */}
          <Card>
            <CardHeader>
              <CardTitle>Reviews</CardTitle>
            </CardHeader>
            <CardContent>
              <div className="flex items-center space-x-2 mb-4">
                <div className="flex items-center">
                  {[1, 2, 3, 4, 5].map((star) => (
                    <StarIcon
                      key={star}
                      className={`h-5 w-5 ${
                        star <= averageRating ? 'text-yellow-400 fill-current' : 'text-gray-300'
                      }`}
                    />
                  ))}
                </div>
                <span className="text-sm text-gray-600">
                  {averageRating.toFixed(1)} ({reviews?.items.length || 0} reviews)
                </span>
              </div>
              
              <div className="space-y-3">
                {reviews?.items.slice(0, 3).map((review) => (
                  <div key={review.id} className="border-b border-gray-200 pb-3 last:border-b-0">
                    <div className="flex items-center justify-between mb-1">
                      <span className="text-sm font-medium">{review.memberName}</span>
                      <div className="flex items-center">
                        {[1, 2, 3, 4, 5].map((star) => (
                          <StarIcon
                            key={star}
                            className={`h-4 w-4 ${
                              star <= review.rating ? 'text-yellow-400 fill-current' : 'text-gray-300'
                            }`}
                          />
                        ))}
                      </div>
                    </div>
                    {review.comment && (
                      <p className="text-sm text-gray-600">{review.comment}</p>
                    )}
                  </div>
                ))}
              </div>
              
              {reviews && reviews.items.length > 3 && (
                <Button variant="outline" size="sm" className="w-full mt-4">
                  View All Reviews
                </Button>
              )}
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
                  Reserve Book
                </Button>
                <Button variant="outline" className="w-full">
                  Add Review
                </Button>
                <Button variant="outline" className="w-full">
                  View History
                </Button>
              </div>
            </CardContent>
          </Card>
        </div>
      </div>
    </div>
  )
}