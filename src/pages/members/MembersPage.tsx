import { useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { Link } from 'react-router-dom'
import { PlusIcon, MagnifyingGlassIcon } from '@heroicons/react/24/outline'
import { membersApi } from '@/services/api'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/Table'
import Button from '@/components/ui/Button'
import Input from '@/components/ui/Input'
import Badge from '@/components/ui/Badge'
import Pagination from '@/components/ui/Pagination'
import LoadingSpinner from '@/components/ui/LoadingSpinner'
import Modal from '@/components/ui/Modal'
import MemberForm from '@/components/forms/MemberForm'
import { Member, MemberStatus, MembershipType } from '@/types'

export default function MembersPage() {
  const [page, setPage] = useState(1)
  const [pageSize] = useState(10)
  const [search, setSearch] = useState('')
  const [showCreateModal, setShowCreateModal] = useState(false)
  const [editingMember, setEditingMember] = useState<Member | null>(null)

  const { data: membersData, isLoading, refetch } = useQuery({
    queryKey: ['members', page, pageSize, search],
    queryFn: () => membersApi.getAll({ page, pageSize, search }),
  })

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

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault()
    setPage(1)
    refetch()
  }

  const handleMemberSaved = () => {
    setShowCreateModal(false)
    setEditingMember(null)
    refetch()
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
          <h1 className="text-2xl font-bold text-gray-900">Members</h1>
          <p className="mt-1 text-sm text-gray-500">
            Manage library members and their accounts
          </p>
        </div>
        <Button onClick={() => setShowCreateModal(true)}>
          <PlusIcon className="h-4 w-4 mr-2" />
          Add Member
        </Button>
      </div>

      {/* Search */}
      <Card>
        <CardContent className="p-6">
          <form onSubmit={handleSearch} className="flex gap-4">
            <div className="flex-1">
              <Input
                placeholder="Search members by name, email, or membership number..."
                value={search}
                onChange={(e) => setSearch(e.target.value)}
                className="w-full"
              />
            </div>
            <Button type="submit" variant="outline">
              <MagnifyingGlassIcon className="h-4 w-4 mr-2" />
              Search
            </Button>
          </form>
        </CardContent>
      </Card>

      {/* Members Table */}
      <Card>
        <CardHeader>
          <CardTitle>Members ({membersData?.totalCount || 0})</CardTitle>
        </CardHeader>
        <CardContent className="p-0">
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Member</TableHead>
                <TableHead>Membership</TableHead>
                <TableHead>Type</TableHead>
                <TableHead>Status</TableHead>
                <TableHead>Books</TableHead>
                <TableHead>Fines</TableHead>
                <TableHead>Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {membersData?.items.map((member) => (
                <TableRow key={member.id}>
                  <TableCell>
                    <div>
                      <Link
                        to={`/members/${member.id}`}
                        className="font-medium text-gray-900 hover:text-primary-600"
                      >
                        {member.firstName} {member.lastName}
                      </Link>
                      <p className="text-sm text-gray-500">{member.email}</p>
                      <p className="text-xs text-gray-400">#{member.membershipNumber}</p>
                    </div>
                  </TableCell>
                  <TableCell>
                    <div className="text-sm">
                      <p>Start: {new Date(member.membershipStartDate).toLocaleDateString()}</p>
                      <p>End: {new Date(member.membershipEndDate).toLocaleDateString()}</p>
                    </div>
                  </TableCell>
                  <TableCell>{getMembershipTypeBadge(member.membershipType)}</TableCell>
                  <TableCell>{getStatusBadge(member.status)}</TableCell>
                  <TableCell>
                    <span className="text-sm">
                      {member.currentBooksCount}/{member.maxBooksAllowed}
                    </span>
                  </TableCell>
                  <TableCell>
                    <span className={`text-sm ${member.totalFinesOwed > 0 ? 'text-red-600 font-medium' : 'text-gray-600'}`}>
                      ${member.totalFinesOwed.toFixed(2)}
                    </span>
                  </TableCell>
                  <TableCell>
                    <div className="flex space-x-2">
                      <Button
                        size="sm"
                        variant="outline"
                        onClick={() => setEditingMember(member)}
                      >
                        Edit
                      </Button>
                      <Link to={`/members/${member.id}`}>
                        <Button size="sm" variant="ghost">
                          View
                        </Button>
                      </Link>
                    </div>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>

          {membersData && (
            <Pagination
              currentPage={page}
              totalPages={Math.ceil(membersData.totalCount / pageSize)}
              onPageChange={setPage}
              totalItems={membersData.totalCount}
              itemsPerPage={pageSize}
            />
          )}
        </CardContent>
      </Card>

      {/* Create/Edit Modal */}
      <Modal
        open={showCreateModal || !!editingMember}
        onClose={() => {
          setShowCreateModal(false)
          setEditingMember(null)
        }}
        title={editingMember ? 'Edit Member' : 'Add New Member'}
        size="lg"
      >
        <MemberForm
          member={editingMember}
          onSave={handleMemberSaved}
          onCancel={() => {
            setShowCreateModal(false)
            setEditingMember(null)
          }}
        />
      </Modal>
    </div>
  )
}