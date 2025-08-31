import { useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { transactionsApi } from '@/services/api'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/Table'
import Button from '@/components/ui/Button'
import Badge from '@/components/ui/Badge'
import Pagination from '@/components/ui/Pagination'
import LoadingSpinner from '@/components/ui/LoadingSpinner'
import { TransactionStatus, TransactionType } from '@/types'
import { Link } from 'react-router-dom'

export default function TransactionsPage() {
  const [page, setPage] = useState(1)
  const [pageSize] = useState(20)
  const [activeTab, setActiveTab] = useState<'all' | 'active' | 'overdue'>('all')

  const { data: transactionsData, isLoading } = useQuery({
    queryKey: ['transactions', activeTab, page, pageSize],
    queryFn: () => {
      switch (activeTab) {
        case 'active':
          return transactionsApi.getActive({ page, pageSize })
        case 'overdue':
          return transactionsApi.getOverdue({ page, pageSize })
        default:
          return { items: [], totalCount: 0, page, pageSize } // Would need a getAll method
      }
    },
  })

  const getStatusBadge = (status: TransactionStatus) => {
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

  const getTypeBadge = (type: TransactionType) => {
    const variants = {
      [TransactionType.Borrow]: 'info',
      [TransactionType.Checkout]: 'info',
      [TransactionType.Return]: 'success',
      [TransactionType.Renewal]: 'warning',
      [TransactionType.Lost]: 'error',
      [TransactionType.Damaged]: 'warning',
    } as const

    return <Badge variant={variants[type]}>{type}</Badge>
  }

  const tabs = [
    { key: 'all', label: 'All Transactions', count: 0 },
    { key: 'active', label: 'Active', count: 0 },
    { key: 'overdue', label: 'Overdue', count: 0 },
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
          <h1 className="text-2xl font-bold text-gray-900">Transactions</h1>
          <p className="mt-1 text-sm text-gray-500">
            Manage book borrowing, returns, and renewals
          </p>
        </div>
        <Button>
          New Transaction
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
              {tab.count > 0 && (
                <span className="ml-2 bg-gray-100 text-gray-900 py-0.5 px-2.5 rounded-full text-xs">
                  {tab.count}
                </span>
              )}
            </button>
          ))}
        </nav>
      </div>

      {/* Transactions Table */}
      <Card>
        <CardHeader>
          <CardTitle>
            {activeTab === 'all' && 'All Transactions'}
            {activeTab === 'active' && 'Active Transactions'}
            {activeTab === 'overdue' && 'Overdue Transactions'}
            {' '}({transactionsData?.totalCount || 0})
          </CardTitle>
        </CardHeader>
        <CardContent className="p-0">
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Transaction #</TableHead>
                <TableHead>Book</TableHead>
                <TableHead>Member</TableHead>
                <TableHead>Type</TableHead>
                <TableHead>Checkout Date</TableHead>
                <TableHead>Due Date</TableHead>
                <TableHead>Status</TableHead>
                <TableHead>Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {transactionsData?.items.map((transaction) => (
                <TableRow key={transaction.id}>
                  <TableCell className="font-mono text-sm">
                    {transaction.transactionNumber}
                  </TableCell>
                  <TableCell>
                    <Link
                      to={`/books/${transaction.bookId}`}
                      className="font-medium text-gray-900 hover:text-primary-600"
                    >
                      {transaction.bookTitle}
                    </Link>
                  </TableCell>
                  <TableCell>
                    <Link
                      to={`/members/${transaction.memberId}`}
                      className="font-medium text-gray-900 hover:text-primary-600"
                    >
                      {transaction.memberName}
                    </Link>
                  </TableCell>
                  <TableCell>{getTypeBadge(transaction.type)}</TableCell>
                  <TableCell>
                    {new Date(transaction.checkoutDate).toLocaleDateString()}
                  </TableCell>
                  <TableCell>
                    <span className={new Date(transaction.dueDate) < new Date() ? 'text-red-600 font-medium' : ''}>
                      {new Date(transaction.dueDate).toLocaleDateString()}
                    </span>
                  </TableCell>
                  <TableCell>{getStatusBadge(transaction.status)}</TableCell>
                  <TableCell>
                    <div className="flex space-x-2">
                      {transaction.status === TransactionStatus.Active && (
                        <>
                          <Button size="sm" variant="outline">
                            Return
                          </Button>
                          <Button size="sm" variant="ghost">
                            Renew
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

          {transactionsData && (
            <Pagination
              currentPage={page}
              totalPages={Math.ceil(transactionsData.totalCount / pageSize)}
              onPageChange={setPage}
              totalItems={transactionsData.totalCount}
              itemsPerPage={pageSize}
            />
          )}
        </CardContent>
      </Card>
    </div>
  )
}