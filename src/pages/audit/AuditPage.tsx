import { useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { auditApi } from '@/services/api'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/Table'
import Button from '@/components/ui/Button'
import Input from '@/components/ui/Input'
import Badge from '@/components/ui/Badge'
import Pagination from '@/components/ui/Pagination'
import LoadingSpinner from '@/components/ui/LoadingSpinner'
import { MagnifyingGlassIcon, DocumentArrowDownIcon } from '@heroicons/react/24/outline'

export default function AuditPage() {
  const [page, setPage] = useState(1)
  const [pageSize] = useState(20)
  const [filters, setFilters] = useState({
    search: '',
    entityType: '',
    action: '',
    userId: '',
    fromDate: '',
    toDate: '',
  })

  const { data: auditLogs, isLoading } = useQuery({
    queryKey: ['audit-logs', page, pageSize, filters],
    queryFn: () => auditApi.getAuditLogs({ 
      page, 
      pageSize, 
      ...Object.fromEntries(Object.entries(filters).filter(([_, v]) => v !== ''))
    }),
  })

  const { data: availableActions } = useQuery({
    queryKey: ['audit-actions'],
    queryFn: auditApi.getAvailableActions,
  })

  const { data: availableEntityTypes } = useQuery({
    queryKey: ['audit-entity-types'],
    queryFn: auditApi.getAvailableEntityTypes,
  })

  const { data: summary } = useQuery({
    queryKey: ['audit-summary'],
    queryFn: () => auditApi.getSummary(),
  })

  const handleFilterChange = (key: string, value: string) => {
    setFilters(prev => ({ ...prev, [key]: value }))
    setPage(1)
  }

  const handleExport = async () => {
    try {
      const blob = await auditApi.exportAuditLogs(filters)
      const url = window.URL.createObjectURL(blob)
      const a = document.createElement('a')
      a.href = url
      a.download = `audit-logs-${new Date().toISOString().split('T')[0]}.csv`
      document.body.appendChild(a)
      a.click()
      window.URL.revokeObjectURL(url)
      document.body.removeChild(a)
    } catch (error) {
      console.error('Export failed:', error)
    }
  }

  const getActionBadge = (action: string) => {
    const variants: Record<string, 'success' | 'warning' | 'error' | 'info' | 'default'> = {
      'CREATE': 'success',
      'UPDATE': 'warning',
      'DELETE': 'error',
      'LOGIN': 'info',
      'LOGOUT': 'default',
    }

    return <Badge variant={variants[action] || 'default'}>{action}</Badge>
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
          <h1 className="text-2xl font-bold text-gray-900">Audit Logs</h1>
          <p className="mt-1 text-sm text-gray-500">
            Track all system activities and changes
          </p>
        </div>
        <Button onClick={handleExport}>
          <DocumentArrowDownIcon className="h-4 w-4 mr-2" />
          Export Logs
        </Button>
      </div>

      {/* Summary Cards */}
      {summary && (
        <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
          <Card>
            <CardContent className="p-4 text-center">
              <p className="text-2xl font-bold text-gray-900">{summary.totalLogs}</p>
              <p className="text-sm text-gray-600">Total Logs</p>
            </CardContent>
          </Card>
          <Card>
            <CardContent className="p-4 text-center">
              <p className="text-2xl font-bold text-blue-600">{summary.todayLogs}</p>
              <p className="text-sm text-gray-600">Today</p>
            </CardContent>
          </Card>
          <Card>
            <CardContent className="p-4 text-center">
              <p className="text-2xl font-bold text-green-600">{summary.thisWeekLogs}</p>
              <p className="text-sm text-gray-600">This Week</p>
            </CardContent>
          </Card>
          <Card>
            <CardContent className="p-4 text-center">
              <p className="text-2xl font-bold text-purple-600">{summary.thisMonthLogs}</p>
              <p className="text-sm text-gray-600">This Month</p>
            </CardContent>
          </Card>
        </div>
      )}

      {/* Filters */}
      <Card>
        <CardContent className="p-6">
          <div className="grid grid-cols-1 md:grid-cols-3 lg:grid-cols-6 gap-4">
            <Input
              placeholder="Search..."
              value={filters.search}
              onChange={(e) => handleFilterChange('search', e.target.value)}
            />
            
            <select
              className="input"
              value={filters.entityType}
              onChange={(e) => handleFilterChange('entityType', e.target.value)}
            >
              <option value="">All Entity Types</option>
              {availableEntityTypes?.map((type) => (
                <option key={type} value={type}>{type}</option>
              ))}
            </select>

            <select
              className="input"
              value={filters.action}
              onChange={(e) => handleFilterChange('action', e.target.value)}
            >
              <option value="">All Actions</option>
              {availableActions?.map((action) => (
                <option key={action} value={action}>{action}</option>
              ))}
            </select>

            <Input
              placeholder="User ID"
              value={filters.userId}
              onChange={(e) => handleFilterChange('userId', e.target.value)}
            />

            <Input
              type="date"
              placeholder="From Date"
              value={filters.fromDate}
              onChange={(e) => handleFilterChange('fromDate', e.target.value)}
            />

            <Input
              type="date"
              placeholder="To Date"
              value={filters.toDate}
              onChange={(e) => handleFilterChange('toDate', e.target.value)}
            />
          </div>
        </CardContent>
      </Card>

      {/* Audit Logs Table */}
      <Card>
        <CardHeader>
          <CardTitle>Audit Logs ({auditLogs?.totalCount || 0})</CardTitle>
        </CardHeader>
        <CardContent className="p-0">
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Timestamp</TableHead>
                <TableHead>User</TableHead>
                <TableHead>Action</TableHead>
                <TableHead>Entity</TableHead>
                <TableHead>IP Address</TableHead>
                <TableHead>Details</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {auditLogs?.items.map((log) => (
                <TableRow key={log.id}>
                  <TableCell className="text-sm">
                    {new Date(log.timestamp).toLocaleString()}
                  </TableCell>
                  <TableCell>
                    <div>
                      <p className="font-medium text-sm">{log.userEmail || 'System'}</p>
                      <p className="text-xs text-gray-500">{log.userType}</p>
                    </div>
                  </TableCell>
                  <TableCell>{getActionBadge(log.action)}</TableCell>
                  <TableCell>
                    <div>
                      <p className="font-medium text-sm">{log.entityType}</p>
                      {log.entityId && (
                        <p className="text-xs text-gray-500">ID: {log.entityId}</p>
                      )}
                    </div>
                  </TableCell>
                  <TableCell className="text-sm font-mono">
                    {log.ipAddress || 'N/A'}
                  </TableCell>
                  <TableCell>
                    <Button size="sm" variant="ghost">
                      View Details
                    </Button>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>

          {auditLogs && (
            <Pagination
              currentPage={page}
              totalPages={Math.ceil(auditLogs.totalCount / pageSize)}
              onPageChange={setPage}
              totalItems={auditLogs.totalCount}
              itemsPerPage={pageSize}
            />
          )}
        </CardContent>
      </Card>
    </div>
  )
}