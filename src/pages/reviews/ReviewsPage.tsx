import { useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { reviewsApi } from '@/services/api'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/Table'
import Button from '@/components/ui/Button'
import Badge from '@/components/ui/Badge'
import Pagination from '@/components/ui/Pagination'
import LoadingSpinner from '@/components/ui/LoadingSpinner'
import { StarIcon, CheckIcon, XMarkIcon } from '@heroicons/react/24/outline'
import { Link } from 'react-router-dom'

export default function ReviewsPage() {
  const [page, setPage] = useState(1)
  const [pageSize] = useState(20)
  const [activeTab, setActiveTab] = useState<'pending' | 'approved'>('pending')

  const { data: reviewsData, isLoading, refetch } = useQuery({
    queryKey: ['reviews', activeTab, page, pageSize],
    queryFn: () => {
      switch (activeTab) {
        case 'pending':
          return reviewsApi.getPending({ page, pageSize })
        case 'approved':
          return reviewsApi.getApproved({ page, pageSize })
        default:
          return reviewsApi.getPending({ page, pageSize })
      }
    },
  })

  const handleApprove = async (reviewId: number) => {
    try {
      await reviewsApi.approve(reviewId)
      refetch()
    } catch (error) {
      console.error('Failed to approve review:', error)
    }
  }

  const handleReject = async (reviewId: number) => {
    try {
      await reviewsApi.reject(reviewId, { reason: 'Inappropriate content' })
      refetch()
    } catch (error) {
      console.error('Failed to reject review:', error)
    }
  }

  const renderStars = (rating: number) => {
    return (
      <div className="flex items-center">
        {[1, 2, 3, 4, 5].map((star) => (
          <StarIcon
            key={star}
            className={`h-4 w-4 ${
              star <= rating ? 'text-yellow-400 fill-current' : 'text-gray-300'
            }`}
          />
        ))}
        <span className="ml-1 text-sm text-gray-600">({rating})</span>
      </div>
    )
  }

  const tabs = [
    { key: 'pending', label: 'Pending Reviews' },
    { key: 'approved', label: 'Approved Reviews' },
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
      <div>
        <h1 className="text-2xl font-bold text-gray-900">Reviews</h1>
        <p className="mt-1 text-sm text-gray-500">
          Manage book reviews and ratings
        </p>
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

      {/* Reviews Table */}
      <Card>
        <CardHeader>
          <CardTitle>
            {activeTab === 'pending' ? 'Pending' : 'Approved'} Reviews ({reviewsData?.totalCount || 0})
          </CardTitle>
        </CardHeader>
        <CardContent className="p-0">
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Book</TableHead>
                <TableHead>Member</TableHead>
                <TableHead>Rating</TableHead>
                <TableHead>Review Date</TableHead>
                <TableHead>Comment</TableHead>
                <TableHead>Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {reviewsData?.items.map((review) => (
                <TableRow key={review.id}>
                  <TableCell>
                    <Link
                      to={`/books/${review.bookId}`}
                      className="font-medium text-gray-900 hover:text-primary-600"
                    >
                      {review.bookTitle}
                    </Link>
                  </TableCell>
                  <TableCell>
                    <Link
                      to={`/members/${review.memberId}`}
                      className="font-medium text-gray-900 hover:text-primary-600"
                    >
                      {review.memberName}
                    </Link>
                  </TableCell>
                  <TableCell>{renderStars(review.rating)}</TableCell>
                  <TableCell>
                    {new Date(review.reviewDate).toLocaleDateString()}
                  </TableCell>
                  <TableCell>
                    <div className="max-w-xs">
                      {review.comment ? (
                        <p className="text-sm text-gray-600 truncate" title={review.comment}>
                          {review.comment}
                        </p>
                      ) : (
                        <span className="text-sm text-gray-400">No comment</span>
                      )}
                    </div>
                  </TableCell>
                  <TableCell>
                    <div className="flex space-x-2">
                      {activeTab === 'pending' ? (
                        <>
                          <Button
                            size="sm"
                            variant="success"
                            onClick={() => handleApprove(review.id)}
                          >
                            <CheckIcon className="h-4 w-4 mr-1" />
                            Approve
                          </Button>
                          <Button
                            size="sm"
                            variant="error"
                            onClick={() => handleReject(review.id)}
                          >
                            <XMarkIcon className="h-4 w-4 mr-1" />
                            Reject
                          </Button>
                        </>
                      ) : (
                        <Button size="sm" variant="ghost">
                          View Details
                        </Button>
                      )}
                    </div>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>

          {reviewsData && (
            <Pagination
              currentPage={page}
              totalPages={Math.ceil(reviewsData.totalCount / pageSize)}
              onPageChange={setPage}
              totalItems={reviewsData.totalCount}
              itemsPerPage={pageSize}
            />
          )}
        </CardContent>
      </Card>

      {/* Create Category Modal */}
      <Modal
        open={showCreateModal || !!editingCategory}
        onClose={() => {
          setShowCreateModal(false)
          setEditingCategory(null)
        }}
        title={editingCategory ? 'Edit Category' : 'Add New Category'}
      >
        <div className="space-y-6">
          <Input
            label="Category Name"
            placeholder="Enter category name"
            defaultValue={editingCategory?.name || ''}
          />
          
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Parent Category (Optional)
            </label>
            <select className="input" defaultValue={editingCategory?.parentCategoryId || ''}>
              <option value="">No parent (Root category)</option>
              {rootCategories
                .filter(cat => cat.id !== editingCategory?.id)
                .map((category) => (
                <option key={category.id} value={category.id}>
                  {category.name}
                </option>
              ))}
            </select>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Description
            </label>
            <textarea
              className="input min-h-[80px]"
              placeholder="Enter category description"
              defaultValue={editingCategory?.description || ''}
            />
          </div>

          <div className="flex items-center">
            <input
              id="isActive"
              type="checkbox"
              className="h-4 w-4 rounded border-gray-300 text-primary-600 focus:ring-primary-600"
              defaultChecked={editingCategory?.isActive ?? true}
            />
            <label htmlFor="isActive" className="ml-2 block text-sm text-gray-900">
              Active category
            </label>
          </div>

          <div className="flex justify-end space-x-3 pt-6 border-t">
            <Button
              variant="outline"
              onClick={() => {
                setShowCreateModal(false)
                setEditingCategory(null)
              }}
            >
              Cancel
            </Button>
            <Button onClick={handleCategorySaved}>
              {editingCategory ? 'Update Category' : 'Create Category'}
            </Button>
          </div>
        </div>
      </Modal>
    </div>
  )
}