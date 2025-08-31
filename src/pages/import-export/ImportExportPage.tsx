import { useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { importExportApi } from '@/services/api'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/Table'
import Button from '@/components/ui/Button'
import Badge from '@/components/ui/Badge'
import LoadingSpinner from '@/components/ui/LoadingSpinner'
import Modal from '@/components/ui/Modal'
import { 
  ArrowUpTrayIcon, 
  ArrowDownTrayIcon, 
  DocumentTextIcon,
  ClockIcon,
  CheckCircleIcon,
  XCircleIcon 
} from '@heroicons/react/24/outline'

export default function ImportExportPage() {
  const [activeTab, setActiveTab] = useState<'import' | 'export'>('import')
  const [showImportModal, setShowImportModal] = useState(false)
  const [showExportModal, setShowExportModal] = useState(false)

  const { data: importHistory, isLoading: importLoading } = useQuery({
    queryKey: ['import-history'],
    queryFn: () => importExportApi.getImportHistory({ pageSize: 20 }),
  })

  const { data: exportHistory, isLoading: exportLoading } = useQuery({
    queryKey: ['export-history'],
    queryFn: () => importExportApi.getExportHistory({ pageSize: 20 }),
  })

  const { data: templates } = useQuery({
    queryKey: ['export-templates'],
    queryFn: importExportApi.getTemplates,
  })

  const { data: supportedFormats } = useQuery({
    queryKey: ['supported-formats'],
    queryFn: importExportApi.getSupportedFormats,
  })

  const getStatusBadge = (status: string) => {
    const variants: Record<string, 'success' | 'warning' | 'error' | 'info' | 'default'> = {
      'Completed': 'success',
      'Processing': 'warning',
      'Failed': 'error',
      'Pending': 'info',
      'Cancelled': 'default',
    }

    return <Badge variant={variants[status] || 'default'}>{status}</Badge>
  }

  const getStatusIcon = (status: string) => {
    switch (status.toLowerCase()) {
      case 'completed':
        return <CheckCircleIcon className="h-5 w-5 text-green-600" />
      case 'processing':
        return <ClockIcon className="h-5 w-5 text-yellow-600" />
      case 'failed':
        return <XCircleIcon className="h-5 w-5 text-red-600" />
      default:
        return <ClockIcon className="h-5 w-5 text-gray-600" />
    }
  }

  const tabs = [
    { key: 'import', label: 'Import Data', icon: ArrowUpTrayIcon },
    { key: 'export', label: 'Export Data', icon: ArrowDownTrayIcon },
  ]

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Import & Export</h1>
          <p className="mt-1 text-sm text-gray-500">
            Import and export library data in various formats
          </p>
        </div>
        <div className="flex space-x-2">
          <Button onClick={() => setShowImportModal(true)}>
            <ArrowUpTrayIcon className="h-4 w-4 mr-2" />
            Import Data
          </Button>
          <Button variant="outline" onClick={() => setShowExportModal(true)}>
            <ArrowDownTrayIcon className="h-4 w-4 mr-2" />
            Export Data
          </Button>
        </div>
      </div>

      {/* Quick Actions */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <Card className="hover:shadow-md transition-shadow cursor-pointer" onClick={() => setShowImportModal(true)}>
          <CardContent className="p-6 text-center">
            <ArrowUpTrayIcon className="h-8 w-8 text-blue-600 mx-auto mb-2" />
            <h3 className="font-medium text-gray-900">Import Books</h3>
            <p className="text-sm text-gray-500 mt-1">Upload book data from CSV or Excel</p>
          </CardContent>
        </Card>

        <Card className="hover:shadow-md transition-shadow cursor-pointer" onClick={() => setShowImportModal(true)}>
          <CardContent className="p-6 text-center">
            <ArrowUpTrayIcon className="h-8 w-8 text-green-600 mx-auto mb-2" />
            <h3 className="font-medium text-gray-900">Import Members</h3>
            <p className="text-sm text-gray-500 mt-1">Bulk import member information</p>
          </CardContent>
        </Card>

        <Card className="hover:shadow-md transition-shadow cursor-pointer" onClick={() => setShowExportModal(true)}>
          <CardContent className="p-6 text-center">
            <ArrowDownTrayIcon className="h-8 w-8 text-purple-600 mx-auto mb-2" />
            <h3 className="font-medium text-gray-900">Export Reports</h3>
            <p className="text-sm text-gray-500 mt-1">Generate comprehensive data exports</p>
          </CardContent>
        </Card>
      </div>

      {/* Tabs */}
      <div className="border-b border-gray-200">
        <nav className="-mb-px flex space-x-8">
          {tabs.map((tab) => (
            <button
              key={tab.key}
              onClick={() => setActiveTab(tab.key as any)}
              className={`py-2 px-1 border-b-2 font-medium text-sm flex items-center space-x-2 ${
                activeTab === tab.key
                  ? 'border-primary-500 text-primary-600'
                  : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
              }`}
            >
              <tab.icon className="h-4 w-4" />
              <span>{tab.label}</span>
            </button>
          ))}
        </nav>
      </div>

      {/* Import History */}
      {activeTab === 'import' && (
        <Card>
          <CardHeader>
            <CardTitle>Import History</CardTitle>
          </CardHeader>
          <CardContent className="p-0">
            {importLoading ? (
              <div className="flex items-center justify-center h-32">
                <LoadingSpinner />
              </div>
            ) : (
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead>File Name</TableHead>
                    <TableHead>Type</TableHead>
                    <TableHead>Status</TableHead>
                    <TableHead>Records</TableHead>
                    <TableHead>Started</TableHead>
                    <TableHead>Completed</TableHead>
                    <TableHead>Actions</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {importHistory?.items.map((job) => (
                    <TableRow key={job.id}>
                      <TableCell>
                        <div className="flex items-center space-x-2">
                          <DocumentTextIcon className="h-4 w-4 text-gray-400" />
                          <span className="font-medium">{job.fileName}</span>
                        </div>
                      </TableCell>
                      <TableCell>{job.importType}</TableCell>
                      <TableCell>
                        <div className="flex items-center space-x-2">
                          {getStatusIcon(job.status)}
                          {getStatusBadge(job.status)}
                        </div>
                      </TableCell>
                      <TableCell>
                        <div className="text-sm">
                          <p>{job.successCount} success</p>
                          <p className="text-red-600">{job.failureCount} failed</p>
                        </div>
                      </TableCell>
                      <TableCell>
                        {new Date(job.startedAt).toLocaleString()}
                      </TableCell>
                      <TableCell>
                        {job.completedAt ? new Date(job.completedAt).toLocaleString() : 'N/A'}
                      </TableCell>
                      <TableCell>
                        <div className="flex space-x-2">
                          <Button size="sm" variant="ghost">
                            View Details
                          </Button>
                          {job.status === 'Failed' && (
                            <Button size="sm" variant="outline">
                              Retry
                            </Button>
                          )}
                        </div>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            )}
          </CardContent>
        </Card>
      )}

      {/* Export History */}
      {activeTab === 'export' && (
        <Card>
          <CardHeader>
            <CardTitle>Export History</CardTitle>
          </CardHeader>
          <CardContent className="p-0">
            {exportLoading ? (
              <div className="flex items-center justify-center h-32">
                <LoadingSpinner />
              </div>
            ) : (
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead>Export Type</TableHead>
                    <TableHead>Format</TableHead>
                    <TableHead>Status</TableHead>
                    <TableHead>Records</TableHead>
                    <TableHead>File Size</TableHead>
                    <TableHead>Started</TableHead>
                    <TableHead>Actions</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {exportHistory?.items.map((job) => (
                    <TableRow key={job.id}>
                      <TableCell>{job.exportType}</TableCell>
                      <TableCell>
                        <Badge variant="default">{job.format.toUpperCase()}</Badge>
                      </TableCell>
                      <TableCell>
                        <div className="flex items-center space-x-2">
                          {getStatusIcon(job.status)}
                          {getStatusBadge(job.status)}
                        </div>
                      </TableCell>
                      <TableCell>{job.totalRecords}</TableCell>
                      <TableCell>
                        {job.fileSize ? `${(job.fileSize / 1024 / 1024).toFixed(2)} MB` : 'N/A'}
                      </TableCell>
                      <TableCell>
                        {new Date(job.startedAt).toLocaleString()}
                      </TableCell>
                      <TableCell>
                        <div className="flex space-x-2">
                          {job.status === 'Completed' && (
                            <Button size="sm" variant="outline">
                              <ArrowDownTrayIcon className="h-4 w-4 mr-1" />
                              Download
                            </Button>
                          )}
                          <Button size="sm" variant="ghost">
                            View Details
                          </Button>
                        </div>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            )}
          </CardContent>
        </Card>
      )}

      {/* Import Modal */}
      <Modal
        open={showImportModal}
        onClose={() => setShowImportModal(false)}
        title="Import Data"
        size="lg"
      >
        <div className="space-y-6">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Import Type
            </label>
            <select className="input">
              <option value="books">Books</option>
              <option value="members">Members</option>
              <option value="categories">Categories</option>
            </select>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              File Format
            </label>
            <select className="input">
              <option value="csv">CSV</option>
              <option value="excel">Excel</option>
              <option value="json">JSON</option>
            </select>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Select File
            </label>
            <input
              type="file"
              className="input"
              accept=".csv,.xlsx,.xls,.json"
            />
          </div>

          <div className="bg-blue-50 border border-blue-200 rounded-lg p-4">
            <h4 className="font-medium text-blue-900 mb-2">Import Guidelines:</h4>
            <ul className="text-sm text-blue-800 space-y-1">
              <li>• Ensure your file follows the required format</li>
              <li>• Maximum file size: 10MB</li>
              <li>• Duplicate entries will be skipped</li>
              <li>• Invalid data will be reported in the results</li>
            </ul>
          </div>

          <div className="flex justify-end space-x-3 pt-6 border-t">
            <Button variant="outline" onClick={() => setShowImportModal(false)}>
              Cancel
            </Button>
            <Button>
              <ArrowUpTrayIcon className="h-4 w-4 mr-2" />
              Start Import
            </Button>
          </div>
        </div>
      </Modal>

      {/* Export Modal */}
      <Modal
        open={showExportModal}
        onClose={() => setShowExportModal(false)}
        title="Export Data"
        size="lg"
      >
        <div className="space-y-6">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Export Type
            </label>
            <select className="input">
              <option value="books">Books</option>
              <option value="members">Members</option>
              <option value="transactions">Transactions</option>
              <option value="fines">Fines</option>
              <option value="reservations">Reservations</option>
            </select>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Format
            </label>
            <select className="input">
              <option value="csv">CSV</option>
              <option value="excel">Excel</option>
              <option value="json">JSON</option>
              <option value="pdf">PDF</option>
            </select>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                From Date (Optional)
              </label>
              <input type="date" className="input" />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                To Date (Optional)
              </label>
              <input type="date" className="input" />
            </div>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Template (Optional)
            </label>
            <select className="input">
              <option value="">Use default format</option>
              {templates?.map((template) => (
                <option key={template.id} value={template.id}>
                  {template.name}
                </option>
              ))}
            </select>
          </div>

          <div className="bg-green-50 border border-green-200 rounded-lg p-4">
            <h4 className="font-medium text-green-900 mb-2">Export Information:</h4>
            <ul className="text-sm text-green-800 space-y-1">
              <li>• Data will be exported based on your current permissions</li>
              <li>• Large exports may take several minutes to complete</li>
              <li>• You'll receive a notification when the export is ready</li>
              <li>• Export files are available for 7 days</li>
            </ul>
          </div>

          <div className="flex justify-end space-x-3 pt-6 border-t">
            <Button variant="outline" onClick={() => setShowExportModal(false)}>
              Cancel
            </Button>
            <Button>
              <ArrowDownTrayIcon className="h-4 w-4 mr-2" />
              Start Export
            </Button>
          </div>
        </div>
      </Modal>
    </div>
  )
}