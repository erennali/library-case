import { useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { librariansApi } from '@/services/api'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/Table'
import Button from '@/components/ui/Button'
import Input from '@/components/ui/Input'
import Badge from '@/components/ui/Badge'
import Pagination from '@/components/ui/Pagination'
import LoadingSpinner from '@/components/ui/LoadingSpinner'
import Modal from '@/components/ui/Modal'
import { PlusIcon, MagnifyingGlassIcon } from '@heroicons/react/24/outline'
import { LibrarianRole, LibrarianStatus } from '@/types'

export default function LibrariansPage() {
  const [page, setPage] = useState(1)
  const [pageSize] = useState(20)
  const [filters, setFilters] = useState({
    query: '',
    role: '',
    status: '',
  })
  const [showCreateModal, setShowCreateModal] = useState(false)

  const { data: librariansData, isLoading } = useQuery({
    queryKey: ['librarians', page, pageSize, filters],
    queryFn: () => librariansApi.getList({ 
      page, 
      pageSize, 
      ...Object.fromEntries(Object.entries(filters).filter(([_, v]) => v !== ''))
    }),
  })

  const { data: stats } = useQuery({
    queryKey: ['librarian-stats'],
    queryFn: librariansApi.getStats,
  })

  const getRoleBadge = (role: LibrarianRole) => {
    const variants = {
      [LibrarianRole.Assistant]: 'default',
      [LibrarianRole.Librarian]: 'info',
      [LibrarianRole.SeniorLibrarian]: 'warning',
      [LibrarianRole.HeadLibrarian]: 'success',
      [LibrarianRole.Administrator]: 'error',
    } as const

    return <Badge variant={variants[role]}>{role}</Badge>
  }

  const getStatusBadge = (status: LibrarianStatus) => {
    const variants = {
      [LibrarianStatus.Active]: 'success',
      [LibrarianStatus.Inactive]: 'default',
      [LibrarianStatus.OnLeave]: 'warning',
      [LibrarianStatus.Terminated]: 'error',
    } as const

    return <Badge variant={variants[status]}>{status}</Badge>
  }

  const handleFilterChange = (key: string, value: string) => {
    setFilters(prev => ({ ...prev, [key]: value }))
    setPage(1)
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
          <h1 className="text-2xl font-bold text-gray-900">Librarians</h1>
          <p className="mt-1 text-sm text-gray-500">
            Manage library staff and their roles
          </p>
        </div>
        <Button onClick={() => setShowCreateModal(true)}>
          <PlusIcon className="h-4 w-4 mr-2" />
          Add Librarian
        </Button>
      </div>

      {/* Stats Cards */}
      {stats && (
        <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
          <Card>
            <CardContent className="p-4 text-center">
              <p className="text-2xl font-bold text-blue-600">{stats.totalLibrarians}</p>
              <p className="text-sm text-gray-600">Total Librarians</p>
            </CardContent>
          </Card>
          <Card>
            <CardContent className="p-4 text-center">
              <p className="text-2xl font-bold text-green-600">{stats.activeLibrarians}</p>
              <p className="text-sm text-gray-600">Active</p>
            </CardContent>
          </Card>
          <Card>
            <CardContent className="p-4 text-center">
              <p className="text-2xl font-bold text-purple-600">{stats.totalTransactionsProcessed}</p>
              <p className="text-sm text-gray-600">Transactions Processed</p>
            </CardContent>
          </Card>
          <Card>
            <CardContent className="p-4 text-center">
              <p className="text-2xl font-bold text-yellow-600">{stats.totalFinesProcessed}</p>
              <p className="text-sm text-gray-600">Fines Processed</p>
            </CardContent>
          </Card>
        </div>
      )}

      {/* Filters */}
      <Card>
        <CardContent className="p-6">
          <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
            <Input
              placeholder="Search librarians..."
              value={filters.query}
              onChange={(e) => handleFilterChange('query', e.target.value)}
            />
            
            <select
              className="input"
              value={filters.role}
              onChange={(e) => handleFilterChange('role', e.target.value)}
            >
              <option value="">All Roles</option>
              {Object.values(LibrarianRole).map((role) => (
                <option key={role} value={role}>{role}</option>
              ))}
            </select>

            <select
              className="input"
              value={filters.status}
              onChange={(e) => handleFilterChange('status', e.target.value)}
            >
              <option value="">All Statuses</option>
              {Object.values(LibrarianStatus).map((status) => (
                <option key={status} value={status}>{status}</option>
              ))}
            </select>

            <Button variant="outline">
              <MagnifyingGlassIcon className="h-4 w-4 mr-2" />
              Search
            </Button>
          </div>
        </CardContent>
      </Card>

      {/* Librarians Table */}
      <Card>
        <CardHeader>
          <CardTitle>Librarians ({librariansData?.totalCount || 0})</CardTitle>
        </CardHeader>
        <CardContent className="p-0">
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Librarian</TableHead>
                <TableHead>Employee #</TableHead>
                <TableHead>Role</TableHead>
                <TableHead>Department</TableHead>
                <TableHead>Hire Date</TableHead>
                <TableHead>Status</TableHead>
                <TableHead>Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {librariansData?.items.map((librarian) => (
                <TableRow key={librarian.id}>
                  <TableCell>
                    <div>
                      <p className="font-medium text-gray-900">
                        {librarian.firstName} {librarian.lastName}
                      </p>
                      <p className="text-sm text-gray-500">{librarian.email}</p>
                      {librarian.phoneNumber && (
                        <p className="text-xs text-gray-400">{librarian.phoneNumber}</p>
                      )}
                    </div>
                  </TableCell>
                  <TableCell className="font-mono text-sm">
                    {librarian.employeeNumber}
                  </TableCell>
                  <TableCell>{getRoleBadge(librarian.role)}</TableCell>
                  <TableCell>{librarian.department || 'N/A'}</TableCell>
                  <TableCell>
                    {new Date(librarian.hireDate).toLocaleDateString()}
                  </TableCell>
                  <TableCell>{getStatusBadge(librarian.status)}</TableCell>
                  <TableCell>
                    <div className="flex space-x-2">
                      <Button size="sm" variant="outline">
                        Edit
                      </Button>
                      {librarian.status === LibrarianStatus.Active ? (
                        <Button size="sm" variant="warning">
                          Deactivate
                        </Button>
                      ) : (
                        <Button size="sm" variant="success">
                          Activate
                        </Button>
                      )}
                      <Button size="sm" variant="ghost">
                        View Activity
                      </Button>
                    </div>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>

          {librariansData && (
            <Pagination
              currentPage={page}
              totalPages={Math.ceil(librariansData.totalCount / pageSize)}
              onPageChange={setPage}
              totalItems={librariansData.totalCount}
              itemsPerPage={pageSize}
            />
          )}
        </CardContent>
      </Card>

      {/* Create Librarian Modal */}
      <Modal
        open={showCreateModal}
        onClose={() => setShowCreateModal(false)}
        title="Add New Librarian"
        size="lg"
      >
        <div className="space-y-6">
          <div className="grid grid-cols-2 gap-4">
            <Input label="Employee Number" />
            <Input label="Department" />
          </div>

          <div className="grid grid-cols-2 gap-4">
            <Input label="First Name" />
            <Input label="Last Name" />
          </div>

          <div className="grid grid-cols-2 gap-4">
            <Input label="Email" type="email" />
            <Input label="Phone Number" type="tel" />
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Role</label>
              <select className="input">
                {Object.values(LibrarianRole).map((role) => (
                  <option key={role} value={role}>{role}</option>
                ))}
              </select>
            </div>
            <Input label="Hire Date" type="date" />
          </div>

          <div className="flex justify-end space-x-3 pt-6 border-t">
            <Button variant="outline" onClick={() => setShowCreateModal(false)}>
              Cancel
            </Button>
            <Button>Create Librarian</Button>
          </div>
        </div>
      </Modal>
    </div>
  )
}