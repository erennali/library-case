import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'
import { useMutation } from '@tanstack/react-query'
import toast from 'react-hot-toast'
import { membersApi } from '@/services/api'
import Button from '@/components/ui/Button'
import Input from '@/components/ui/Input'
import { Member, MemberStatus, MembershipType } from '@/types'

const memberSchema = z.object({
  membershipNumber: z.string().min(1, 'Membership number is required'),
  firstName: z.string().min(1, 'First name is required'),
  lastName: z.string().min(1, 'Last name is required'),
  email: z.string().email('Invalid email address'),
  phoneNumber: z.string().optional(),
  address: z.string().optional(),
  dateOfBirth: z.string().min(1, 'Date of birth is required'),
  membershipType: z.nativeEnum(MembershipType),
  membershipStartDate: z.string().min(1, 'Membership start date is required'),
  membershipEndDate: z.string().min(1, 'Membership end date is required'),
  status: z.nativeEnum(MemberStatus),
  maxBooksAllowed: z.number().min(1, 'Max books must be at least 1'),
  maxFineLimit: z.number().min(0, 'Max fine limit cannot be negative'),
})

type MemberFormData = z.infer<typeof memberSchema>

interface MemberFormProps {
  member?: Member | null
  onSave: () => void
  onCancel: () => void
}

export default function MemberForm({ member, onSave, onCancel }: MemberFormProps) {
  const isEditing = !!member

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<MemberFormData>({
    resolver: zodResolver(memberSchema),
    defaultValues: member ? {
      membershipNumber: member.membershipNumber,
      firstName: member.firstName,
      lastName: member.lastName,
      email: member.email,
      phoneNumber: member.phoneNumber || '',
      address: member.address || '',
      dateOfBirth: member.dateOfBirth.split('T')[0],
      membershipType: member.membershipType,
      membershipStartDate: member.membershipStartDate.split('T')[0],
      membershipEndDate: member.membershipEndDate.split('T')[0],
      status: member.status,
      maxBooksAllowed: member.maxBooksAllowed,
      maxFineLimit: member.maxFineLimit,
    } : {
      membershipType: MembershipType.Regular,
      status: MemberStatus.Active,
      maxBooksAllowed: 5,
      maxFineLimit: 50,
      membershipStartDate: new Date().toISOString().split('T')[0],
      membershipEndDate: new Date(Date.now() + 365 * 24 * 60 * 60 * 1000).toISOString().split('T')[0],
    },
  })

  const createMutation = useMutation({
    mutationFn: membersApi.create,
    onSuccess: () => {
      toast.success('Member created successfully!')
      onSave()
    },
    onError: (error: any) => {
      toast.error(error.response?.data?.message || 'Failed to create member')
    },
  })

  const updateMutation = useMutation({
    mutationFn: ({ id, data }: { id: number; data: Partial<Member> }) => membersApi.update(id, data),
    onSuccess: () => {
      toast.success('Member updated successfully!')
      onSave()
    },
    onError: (error: any) => {
      toast.error(error.response?.data?.message || 'Failed to update member')
    },
  })

  const onSubmit = (data: MemberFormData) => {
    const memberData = {
      ...data,
      currentBooksCount: member?.currentBooksCount || 0,
      totalFinesOwed: member?.totalFinesOwed || 0,
    }

    if (isEditing && member) {
      updateMutation.mutate({ id: member.id, data: memberData })
    } else {
      createMutation.mutate(memberData)
    }
  }

  const isLoading = createMutation.isPending || updateMutation.isPending

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
      <div className="grid grid-cols-2 gap-4">
        <Input
          label="Membership Number"
          {...register('membershipNumber')}
          error={errors.membershipNumber?.message}
          disabled={isEditing}
        />
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Status
          </label>
          <select
            className="input"
            {...register('status')}
          >
            {Object.values(MemberStatus).map((status) => (
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

      <div className="grid grid-cols-2 gap-4">
        <Input
          label="First Name"
          {...register('firstName')}
          error={errors.firstName?.message}
        />
        <Input
          label="Last Name"
          {...register('lastName')}
          error={errors.lastName?.message}
        />
      </div>

      <div className="grid grid-cols-2 gap-4">
        <Input
          label="Email"
          type="email"
          {...register('email')}
          error={errors.email?.message}
        />
        <Input
          label="Phone Number"
          type="tel"
          {...register('phoneNumber')}
          error={errors.phoneNumber?.message}
        />
      </div>

      <Input
        label="Address"
        {...register('address')}
        error={errors.address?.message}
      />

      <div className="grid grid-cols-2 gap-4">
        <Input
          label="Date of Birth"
          type="date"
          {...register('dateOfBirth')}
          error={errors.dateOfBirth?.message}
        />
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Membership Type
          </label>
          <select
            className="input"
            {...register('membershipType')}
          >
            {Object.values(MembershipType).map((type) => (
              <option key={type} value={type}>
                {type}
              </option>
            ))}
          </select>
          {errors.membershipType && (
            <p className="mt-1 text-sm text-error-600">{errors.membershipType.message}</p>
          )}
        </div>
      </div>

      <div className="grid grid-cols-2 gap-4">
        <Input
          label="Membership Start Date"
          type="date"
          {...register('membershipStartDate')}
          error={errors.membershipStartDate?.message}
        />
        <Input
          label="Membership End Date"
          type="date"
          {...register('membershipEndDate')}
          error={errors.membershipEndDate?.message}
        />
      </div>

      <div className="grid grid-cols-2 gap-4">
        <Input
          label="Max Books Allowed"
          type="number"
          min="1"
          {...register('maxBooksAllowed', { valueAsNumber: true })}
          error={errors.maxBooksAllowed?.message}
        />
        <Input
          label="Max Fine Limit"
          type="number"
          min="0"
          step="0.01"
          {...register('maxFineLimit', { valueAsNumber: true })}
          error={errors.maxFineLimit?.message}
        />
      </div>

      <div className="flex justify-end space-x-3 pt-6 border-t">
        <Button type="button" variant="outline" onClick={onCancel}>
          Cancel
        </Button>
        <Button type="submit" loading={isLoading}>
          {isEditing ? 'Update Member' : 'Create Member'}
        </Button>
      </div>
    </form>
  )
}