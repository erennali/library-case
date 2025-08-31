import { useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { categoriesApi } from '@/services/api'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/Table'
import Button from '@/components/ui/Button'
import Input from '@/components/ui/Input'
import Badge from '@/components/ui/Badge'
import LoadingSpinner from '@/components/ui/LoadingSpinner'
import Modal from '@/components/ui/Modal'
import { PlusIcon, FolderIcon, FolderOpenIcon } from '@heroicons/react/24/outline'
import { Category } from '@/types'

export default function CategoriesPage() {
  const [showCreateModal, setShowCreateModal] = useState(false)
  const [editingCategory, setEditingCategory] = useState<Category | null>(null)

  const { data: categoriesData, isLoading, refetch } = useQuery({
    queryKey: ['categories'],
    queryFn: () => categoriesApi.getAll({ pageSize: 100 }),
  })

  const handleCategorySaved = () => {
    setShowCreateModal(false)
    setEditingCategory(null)
    refetch()
  }

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-64">
        <LoadingSpinner size="lg" />
      </div>
    )
  }

  // Organize categories into tree structure
  const rootCategories = categoriesData?.items.filter(cat => !cat.parentCategoryId) || []
  const subCategories = categoriesData?.items.filter(cat => cat.parentCategoryId) || []

  const getCategoryWithChildren = (category: Category) => {
    const children = subCategories.filter(sub => sub.parentCategoryId === category.id)
    return { ...category, children }
  }

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Categories</h1>
          <p className="mt-1 text-sm text-gray-500">
            Organize your book collection with categories
          </p>
        </div>
        <Button onClick={() => setShowCreateModal(true)}>
          <PlusIcon className="h-4 w-4 mr-2" />
          Add Category
        </Button>
      </div>

      {/* Categories Tree */}
      <Card>
        <CardHeader>
          <CardTitle>Category Structure ({categoriesData?.totalCount || 0})</CardTitle>
        </CardHeader>
        <CardContent className="p-0">
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Category</TableHead>
                <TableHead>Description</TableHead>
                <TableHead>Books Count</TableHead>
                <TableHead>Status</TableHead>
                <TableHead>Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {rootCategories.map((category) => {
                const categoryWithChildren = getCategoryWithChildren(category)
                return (
                  <React.Fragment key={category.id}>
                    {/* Parent Category */}
                    <TableRow>
                      <TableCell>
                        <div className="flex items-center space-x-2">
                          <FolderIcon className="h-5 w-5 text-primary-600" />
                          <span className="font-medium text-gray-900">{category.name}</span>
                        </div>
                      </TableCell>
                      <TableCell>{category.description || 'N/A'}</TableCell>
                      <TableCell>
                        <Badge variant="info">{category.books?.length || 0}</Badge>
                      </TableCell>
                      <TableCell>
                        <Badge variant={category.isActive ? 'success' : 'default'}>
                          {category.isActive ? 'Active' : 'Inactive'}
                        </Badge>
                      </TableCell>
                      <TableCell>
                        <div className="flex space-x-2">
                          <Button
                            size="sm"
                            variant="outline"
                            onClick={() => setEditingCategory(category)}
                          >
                            Edit
                          </Button>
                          <Button size="sm" variant="ghost">
                            Add Subcategory
                          </Button>
                        </div>
                      </TableCell>
                    </TableRow>

                    {/* Child Categories */}
                    {categoryWithChildren.children.map((child) => (
                      <TableRow key={child.id} className="bg-gray-50">
                        <TableCell>
                          <div className="flex items-center space-x-2 ml-6">
                            <FolderOpenIcon className="h-4 w-4 text-gray-400" />
                            <span className="text-gray-700">{child.name}</span>
                          </div>
                        </TableCell>
                        <TableCell>{child.description || 'N/A'}</TableCell>
                        <TableCell>
                          <Badge variant="info">{child.books?.length || 0}</Badge>
                        </TableCell>
                        <TableCell>
                          <Badge variant={child.isActive ? 'success' : 'default'}>
                            {child.isActive ? 'Active' : 'Inactive'}
                          </Badge>
                        </TableCell>
                        <TableCell>
                          <div className="flex space-x-2">
                            <Button
                              size="sm"
                              variant="outline"
                              onClick={() => setEditingCategory(child)}
                            >
                              Edit
                            </Button>
                            <Button size="sm" variant="ghost">
                              Delete
                            </Button>
                          </div>
                        </TableCell>
                      </TableRow>
                    ))}
                  </React.Fragment>
                )
              })}
            </TableBody>
          </Table>
        </CardContent>
      </Card>

      {/* Create/Edit Modal */}
      <Modal
        open={showCreateModal || !!editingCategory}
        onClose={() => {
          setShowCreateModal(false)
          setEditingCategory(null)
        }}
        title={editingCategory ? 'Edit Category' : 'Add New Category'}
      >
        <div className="space-y-6">
          <Input label="Category Name" placeholder="Enter category name" />
          
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Parent Category (Optional)
            </label>
            <select className="input">
              <option value="">No parent (Root category)</option>
              {rootCategories.map((category) => (
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