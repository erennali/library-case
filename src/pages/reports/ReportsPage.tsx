import { useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { reportsApi } from '@/services/api'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/Table'
import Button from '@/components/ui/Button'
import Badge from '@/components/ui/Badge'
import Pagination from '@/components/ui/Pagination'
import LoadingSpinner from '@/components/ui/LoadingSpinner'
import Modal from '@/components/ui/Modal'
import { PlusIcon, DocumentArrowDownIcon } from '@heroicons/react/24/outline'

export default function ReportsPage() {
  const [page, setPage] = useState(1)
  const [pageSize] = useState(20)
  const [showCreateModal, setShowCreateModal] = useState(false)

  const { data: reportsData, isLoading } = useQuery({
    queryKey: ['reports', page, pageSize],
    queryFn: () => reportsApi.getList({ page, pageSize }),
  })

  const { data: reportTypes } = useQuery({
    queryKey: ['report-types'],
    queryFn: reportsApi.getAvailableTypes,
  })

  const getStatusBadge = (status: string) => {
    const variants: Record<string, 'success' | 'warning' | 'error' | 'info' | 'default'> = {
      'Completed': 'success',
      'Processing': 'warning',
      'Failed': 'error',
      'Pending': 'info',
      'Scheduled': 'default',
    }

    return <Badge variant={variants[status] || 'default'}>{status}</Badge>
  }

  const handleDownload = async (reportId: number) => {
    try {
      const blob = await reportsApi.download(reportId)
      const url = window.URL.createObjectURL(blob)
      const a = document.createElement('a')
      a.href = url
      a.download = `report-${reportId}.pdf`
      document.body.appendChild(a)
      a.click()
      window.URL.revokeObjectURL(url)
      document.body.removeChild(a)
    } catch (error) {
      console.error('Download failed:', error)
    }
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
          <h1 className="text-2xl font-bold text-gray-900">Reports</h1>
          <p className="mt-1 text-sm text-gray-500">
            Generate and manage library reports
          </p>
        </div>
        <Button onClick={() => setShowCreateModal(true)}>
          <PlusIcon className="h-4 w-4 mr-2" />
          Generate Report
        </Button>
      </div>

      {/* Quick Report Cards */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <Card className="hover:shadow-md transition-shadow cursor-pointer">
          <CardContent className="p-6">
            <div className="text-center">
              <DocumentArrowDownIcon className="h-8 w-8 text-primary-600 mx-auto mb-2" />
              <h3 className="font-medium text-gray-900">Circulation Report</h3>
              <p className="text-sm text-gray-500 mt-1">Book borrowing and return statistics</p>
              <Button size="sm" className="mt-3">Generate</Button>
            </div>
          </CardContent>
        </Card>

        <Card className="hover:shadow-md transition-shadow cursor-pointer">
          <CardContent className="p-6">
            <div className="text-center">
              <DocumentArrowDownIcon className="h-8 w-8 text-green-600 mx-auto mb-2" />
              <h3 className="font-medium text-gray-900">Member Activity</h3>
              <p className="text-sm text-gray-500 mt-1">Member engagement and usage patterns</p>
              <Button size="sm" className="mt-3">Generate</Button>
            </div>
          </CardContent>
        </Card>

        <Card className="hover:shadow-md transition-shadow cursor-pointer">
          <CardContent className="p-6">
            <div className="text-center">
              <DocumentArrowDownIcon className="h-8 w-8 text-red-600 mx-auto mb-2" />
              <h3 className="font-medium text-gray-900">Overdue Summary</h3>
              <p className="text-sm text-gray-500 mt-1">Overdue books and fine details</p>
              <Button size="sm" className="mt-3">Generate</Button>
            </div>
          </CardContent>
        </Card>
      </div>

      {/* Reports Table */}
      <Card>
        <CardHeader>
          <CardTitle>Generated Reports ({reportsData?.totalCount || 0})</CardTitle>
        </CardHeader>
        <CardContent className="p-0">
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Report Type</TableHead>
                <TableHead>Format</TableHead>
                <TableHead>Status</TableHead>
                <TableHead>Generated At</TableHead>
                <TableHead>File Size</TableHead>
                <TableHead>Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {reportsData?.items.map((report) => (
                <TableRow key={report.id}>
                  <TableCell className="font-medium">{report.reportType}</TableCell>
                  <TableCell>
                    <Badge variant="default">{report.format.toUpperCase()}</Badge>
                  </TableCell>
                  <TableCell>{getStatusBadge(report.status)}</TableCell>
                  <TableCell>
                    {report.generatedAt 
                      ? new Date(report.generatedAt).toLocaleString()
                      : 'Not generated'
                    }
                  </TableCell>
                  <TableCell>
                    {report.fileSize 
                      ? `${(report.fileSize / 1024 / 1024).toFixed(2)} MB`
                      : 'N/A'
                    }
                  </TableCell>
                  <TableCell>
                    <div className="flex space-x-2">
                      {report.status === 'Completed' && (
                        <Button
                          size="sm"
                          variant="outline"
                          onClick={() => handleDownload(report.id)}
                        >
                          <DocumentArrowDownIcon className="h-4 w-4 mr-1" />
                          Download
                        </Button>
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

          {reportsData && (
            <Pagination
              currentPage={page}
              totalPages={Math.ceil(reportsData.totalCount / pageSize)}
              onPageChange={setPage}
              totalItems={reportsData.totalCount}
              itemsPerPage={pageSize}
            />
          )}
        </CardContent>
      </Card>

      {/* Generate Report Modal */}
      <Modal
        open={showCreateModal}
        onClose={() => setShowCreateModal(false)}
        title="Generate New Report"
        size="lg"
      >
        <div className="space-y-6">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Report Type
            </label>
            <select className="input">
              <option value="">Select report type</option>
              {reportTypes?.map((type) => (
                <option key={type.name} value={type.name}>
                  {type.displayName}
                </option>
              ))}
            </select>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Format
            </label>
            <select className="input">
              <option value="pdf">PDF</option>
              <option value="excel">Excel</option>
              <option value="csv">CSV</option>
            </select>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                From Date
              </label>
              <input type="date" className="input" />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                To Date
              </label>
              <input type="date" className="input" />
            </div>
          </div>

          <div className="flex justify-end space-x-3 pt-6 border-t">
            <Button variant="outline" onClick={() => setShowCreateModal(false)}>
              Cancel
            </Button>
            <Button>
              Generate Report
            </Button>
          </div>
        </div>
      </Modal>
    </div>
  )
}