import { useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { finesApi } from '@/services/api'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/Table'
import Button from '@/components/ui/Button'
import Badge from '@/components/ui/Badge'
import Pagination from '@/components/ui/Pagination'
import LoadingSpinner from '@/components/ui/LoadingSpinner'
import { FineStatus, FineType } from '@/types'
import { Link } from 'react-router-dom'

export default function FinesPage() {
  const [page, setPage] = useState(1)
  const [pageSize] = useState(20)
  const [activeTab, setActiveTab] = useState<'pending' | 'overdue'>('pending')

  const { data: finesData, isLoading } = useQuery({
    queryKey: ['fines', activeTab, page, pageSize],
    queryFn: () => {
      switch (activeTab) {
        case 'pending':
          return finesApi.getPending({ page, pageSize })
        case 'overdue':
          return finesApi.getOverdue({ page, pageSize })
        default:
          return finesApi.getPending({ page, pageSize })
      }
    },
  })

  const { data: overallSummary } = useQuery({
    queryKey: ['fines-summary'],
    queryFn: finesApi.getOverallSummary,
  })

  const getStatusBadge = (status: FineStatus) => {
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

  const getTypeBadge = (type: FineType) => {
    const variants = {
      [FineType.OverdueBook]: 'warning',
      [FineType.DamagedBook]: 'error',
      [FineType.LostBook]: 'error',
      [FineType.LateReturn]: 'warning',
      [FineType.Other]: 'default',
    } as const

    return <Badge variant={variants[type]}>{type}</Badge>
  }

  const tabs = [
    { key: 'pending', label: 'Pending Fines' },
    { key: 'overdue', label: 'Overdue Fines' },
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
          <h1 className="text-2xl font-bold text-gray-900">Fines</h1>
          <p className="mt-1 text-sm text-gray-500">
            Manage library fines and payments
          </p>
        </div>
      </div>

      {/* Summary Cards */}
      {overallSummary && (
        <div className="grid grid-cols-1 md:grid-cols-4 gap-6">
          <Card>
            <CardContent className="p-6">
              <div className="text-center">
                <p className="text-2xl font-bold text-gray-900">{overallSummary.totalFines}</p>
                <p className="text-sm text-gray-600">Total Fines</p>
              </div>
            </CardContent>
          </Card>
          <Card>
            <CardContent className="p-6">
              <div className="text-center">
                <p className="text-2xl font-bold text-red-600">${overallSummary.pendingAmount.toFixed(2)}</p>
                <p className="text-sm text-gray-600">Pending Amount</p>
              </div>
            </CardContent>
          </Card>
          <Card>
            <CardContent className="p-6">
              <div className="text-center">
                <p className="text-2xl font-bold text-green-600">${overallSummary.paidAmount.toFixed(2)}</p>
                <p className="text-sm text-gray-600">Paid Amount</p>
              </div>
            </CardContent>
          </Card>
          <Card>
            <CardContent className="p-6">
              <div className="text-center">
                <p className="text-2xl font-bold text-gray-900">${overallSummary.averageFineAmount.toFixed(2)}</p>
                <p className="text-sm text-gray-600">Average Fine</p>
              </div>
            </CardContent>
          </Card>
        </div>
      )}

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

      {/* Fines Table */}
      <Card>
        <CardHeader>
          <CardTitle>
            {activeTab === 'pending' ? 'Pending' : 'Overdue'} Fines ({finesData?.totalCount || 0})
          </CardTitle>
        </CardHeader>
        <CardContent className="p-0">
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Fine #</TableHead>
                <TableHead>Member</TableHead>
                <TableHead>Type</TableHead>
                <TableHead>Amount</TableHead>
                <TableHead>Issue Date</TableHead>
                <TableHead>Due Date</TableHead>
                <TableHead>Status</TableHead>
                <TableHead>Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {finesData?.items.map((fine) => (
                <TableRow key={fine.id}>
                  <TableCell className="font-mono text-sm">
                    {fine.fineNumber}
                  </TableCell>
                  <TableCell>
                    <Link
                      to={`/members/${fine.memberId}`}
                      className="font-medium text-gray-900 hover:text-primary-600"
                    >
                      {fine.memberName}
                    </Link>
                  </TableCell>
                  <TableCell>{getTypeBadge(fine.type)}</TableCell>
                  <TableCell>
                    <span className="font-medium">${fine.amount.toFixed(2)}</span>
                    {fine.paidAmount && fine.paidAmount > 0 && (
                      <p className="text-xs text-green-600">
                        Paid: ${fine.paidAmount.toFixed(2)}
                      </p>
                    )}
                  </TableCell>
                  <TableCell>
                    {new Date(fine.issueDate).toLocaleDateString()}
                  </TableCell>
                  <TableCell>
                    {fine.dueDate ? (
                      <span className={new Date(fine.dueDate) < new Date() ? 'text-red-600 font-medium' : ''}>
                        {new Date(fine.dueDate).toLocaleDateString()}
                      </span>
                    ) : (
                      'N/A'
                    )}
                  </TableCell>
                  <TableCell>{getStatusBadge(fine.status)}</TableCell>
                  <TableCell>
                    <div className="flex space-x-2">
                      {(fine.status === FineStatus.Pending || fine.status === FineStatus.Unpaid) && (
                        <>
                          <Button size="sm" variant="outline">
                            Pay
                          </Button>
                          <Button size="sm" variant="ghost">
                            Waive
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

          {finesData && (
            <Pagination
              currentPage={page}
              totalPages={Math.ceil(finesData.totalCount / pageSize)}
              onPageChange={setPage}
              totalItems={finesData.totalCount}
              itemsPerPage={pageSize}
            />
          )}
        </CardContent>
      </Card>
    </div>
  )
}