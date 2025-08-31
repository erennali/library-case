import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'
import { useMutation, useQuery } from '@tanstack/react-query'
import toast from 'react-hot-toast'
import { booksApi, categoriesApi } from '@/services/api'
import Button from '@/components/ui/Button'
import Input from '@/components/ui/Input'
import { Book, BookStatus } from '@/types'

const bookSchema = z.object({
  isbn: z.string().min(1, 'ISBN is required'),
  title: z.string().min(1, 'Title is required'),
  author: z.string().min(1, 'Author is required'),
  publisher: z.string().optional(),
  publicationDate: z.string().optional(),
  description: z.string().optional(),
  categoryId: z.number().min(1, 'Category is required'),
  totalCopies: z.number().min(1, 'Total copies must be at least 1'),
  availableCopies: z.number().min(0, 'Available copies cannot be negative'),
  language: z.string().optional(),
  pageCount: z.number().min(1, 'Page count must be at least 1'),
  imageUrl: z.string().url().optional().or(z.literal('')),
  price: z.number().min(0).optional(),
  status: z.nativeEnum(BookStatus),
})

type BookFormData = z.infer<typeof bookSchema>

interface BookFormProps {
  book?: Book | null
  onSave: () => void
  onCancel: () => void
}

export default function BookForm({ book, onSave, onCancel }: BookFormProps) {
  const isEditing = !!book

  const { data: categoriesData } = useQuery({
    queryKey: ['categories'],
    queryFn: () => categoriesApi.getAll({ pageSize: 100 }),
  })

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<BookFormData>({
    resolver: zodResolver(bookSchema),
    defaultValues: book ? {
      isbn: book.isbn,
      title: book.title,
      author: book.author,
      publisher: book.publisher || '',
      publicationDate: book.publicationDate ? book.publicationDate.split('T')[0] : '',
      description: book.description || '',
      categoryId: book.categoryId,
      totalCopies: book.totalCopies,
      availableCopies: book.availableCopies,
      language: book.language || '',
      pageCount: book.pageCount,
      imageUrl: book.imageUrl || '',
      price: book.price || 0,
      status: book.status,
    } : {
      status: BookStatus.Available,
      totalCopies: 1,
      availableCopies: 1,
      pageCount: 1,
    },
  })

  const createMutation = useMutation({
    mutationFn: booksApi.create,
    onSuccess: () => {
      toast.success('Book created successfully!')
      onSave()
    },
    onError: (error: any) => {
      toast.error(error.response?.data?.message || 'Failed to create book')
    },
  })

  const updateMutation = useMutation({
    mutationFn: ({ id, data }: { id: number; data: Partial<Book> }) => booksApi.update(id, data),
    onSuccess: () => {
      toast.success('Book updated successfully!')
      onSave()
    },
    onError: (error: any) => {
      toast.error(error.response?.data?.message || 'Failed to update book')
    },
  })

  const onSubmit = (data: BookFormData) => {
    if (isEditing && book) {
      updateMutation.mutate({ id: book.id, data })
    } else {
      createMutation.mutate(data)
    }
  }

  const isLoading = createMutation.isPending || updateMutation.isPending

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
      <div className="grid grid-cols-2 gap-4">
        <Input
          label="ISBN"
          {...register('isbn')}
          error={errors.isbn?.message}
        />
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Category
          </label>
          <select
            className="input"
            {...register('categoryId', { valueAsNumber: true })}
          >
            <option value="">Select a category</option>
            {categoriesData?.items.map((category) => (
              <option key={category.id} value={category.id}>
                {category.name}
              </option>
            ))}
          </select>
          {errors.categoryId && (
            <p className="mt-1 text-sm text-error-600">{errors.categoryId.message}</p>
          )}
        </div>
      </div>

      <Input
        label="Title"
        {...register('title')}
        error={errors.title?.message}
      />

      <Input
        label="Author"
        {...register('author')}
        error={errors.author?.message}
      />

      <div className="grid grid-cols-2 gap-4">
        <Input
          label="Publisher"
          {...register('publisher')}
          error={errors.publisher?.message}
        />
        <Input
          label="Publication Date"
          type="date"
          {...register('publicationDate')}
          error={errors.publicationDate?.message}
        />
      </div>

      <div>
        <label className="block text-sm font-medium text-gray-700 mb-1">
          Description
        </label>
        <textarea
          className="input min-h-[100px]"
          {...register('description')}
        />
        {errors.description && (
          <p className="mt-1 text-sm text-error-600">{errors.description.message}</p>
        )}
      </div>

      <div className="grid grid-cols-3 gap-4">
        <Input
          label="Total Copies"
          type="number"
          min="1"
          {...register('totalCopies', { valueAsNumber: true })}
          error={errors.totalCopies?.message}
        />
        <Input
          label="Available Copies"
          type="number"
          min="0"
          {...register('availableCopies', { valueAsNumber: true })}
          error={errors.availableCopies?.message}
        />
        <Input
          label="Page Count"
          type="number"
          min="1"
          {...register('pageCount', { valueAsNumber: true })}
          error={errors.pageCount?.message}
        />
      </div>

      <div className="grid grid-cols-2 gap-4">
        <Input
          label="Language"
          {...register('language')}
          error={errors.language?.message}
        />
        <Input
          label="Price"
          type="number"
          step="0.01"
          min="0"
          {...register('price', { valueAsNumber: true })}
          error={errors.price?.message}
        />
      </div>

      <div className="grid grid-cols-2 gap-4">
        <Input
          label="Image URL"
          type="url"
          {...register('imageUrl')}
          error={errors.imageUrl?.message}
        />
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Status
          </label>
          <select
            className="input"
            {...register('status')}
          >
            {Object.values(BookStatus).map((status) => (
              <option key={status} value={status}>
                {status}
              </option>
            ))}
          </select>
          {errors.status && (
            <p className="mt-1 text-sm text-error-600">{errors.status.message}</p>
          )}
        </div>
      </div>

      <div className="flex justify-end space-x-3 pt-6 border-t">
        <Button type="button" variant="outline" onClick={onCancel}>
          Cancel
        </Button>
        <Button type="submit" loading={isLoading}>
          {isEditing ? 'Update Book' : 'Create Book'}
        </Button>
      </div>
    </form>
  )
}