import { useState } from 'react'
import { useQuery, useMutation } from '@tanstack/react-query'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'
import toast from 'react-hot-toast'
import { settingsApi } from '@/services/api'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card'
import Button from '@/components/ui/Button'
import Input from '@/components/ui/Input'
import LoadingSpinner from '@/components/ui/LoadingSpinner'
import { LibrarySettings } from '@/types'

const librarySettingsSchema = z.object({
  libraryName: z.string().min(1, 'Library name is required'),
  libraryAddress: z.string().min(1, 'Library address is required'),
  libraryPhone: z.string().min(1, 'Library phone is required'),
  libraryEmail: z.string().email('Invalid email address'),
  libraryWebsite: z.string().url('Invalid website URL').optional().or(z.literal('')),
  currency: z.string().min(1, 'Currency is required'),
  maxBooksPerMember: z.number().min(1, 'Must be at least 1'),
  defaultLoanDays: z.number().min(1, 'Must be at least 1'),
  maxRenewals: z.number().min(0, 'Cannot be negative'),
  dailyFineAmount: z.number().min(0, 'Cannot be negative'),
  maxFineLimit: z.number().min(0, 'Cannot be negative'),
  allowReservations: z.boolean(),
  reservationExpiryDays: z.number().min(1, 'Must be at least 1'),
  requireReviewApproval: z.boolean(),
})

type LibrarySettingsFormData = z.infer<typeof librarySettingsSchema>

export default function SettingsPage() {
  const [activeTab, setActiveTab] = useState<'library' | 'fines' | 'notifications'>('library')

  const { data: librarySettings, isLoading } = useQuery({
    queryKey: ['library-settings'],
    queryFn: settingsApi.getLibrarySettings,
  })

  const { data: fineSettings } = useQuery({
    queryKey: ['fine-settings'],
    queryFn: settingsApi.getFineSettings,
  })

  const { data: notificationSettings } = useQuery({
    queryKey: ['notification-settings'],
    queryFn: settingsApi.getNotificationSettings,
  })

  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<LibrarySettingsFormData>({
    resolver: zodResolver(librarySettingsSchema),
  })

  const updateLibrarySettingsMutation = useMutation({
    mutationFn: settingsApi.updateLibrarySettings,
    onSuccess: () => {
      toast.success('Library settings updated successfully!')
    },
    onError: (error: any) => {
      toast.error(error.response?.data?.message || 'Failed to update settings')
    },
  })

  // Reset form when data loads
  useState(() => {
    if (librarySettings) {
      reset(librarySettings)
    }
  })

  const onSubmit = (data: LibrarySettingsFormData) => {
    updateLibrarySettingsMutation.mutate(data)
  }

  const tabs = [
    { key: 'library', label: 'Library Settings' },
    { key: 'fines', label: 'Fine Settings' },
    { key: 'notifications', label: 'Notification Settings' },
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
        <h1 className="text-2xl font-bold text-gray-900">Settings</h1>
        <p className="mt-1 text-sm text-gray-500">
          Configure your library management system
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

      {/* Library Settings */}
      {activeTab === 'library' && (
        <Card>
          <CardHeader>
            <CardTitle>Library Information</CardTitle>
          </CardHeader>
          <CardContent>
            <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
              <div className="grid grid-cols-2 gap-4">
                <Input
                  label="Library Name"
                  {...register('libraryName')}
                  error={errors.libraryName?.message}
                />
                <Input
                  label="Currency"
                  {...register('currency')}
                  error={errors.currency?.message}
                />
              </div>

              <Input
                label="Library Address"
                {...register('libraryAddress')}
                error={errors.libraryAddress?.message}
              />

              <div className="grid grid-cols-2 gap-4">
                <Input
                  label="Phone Number"
                  type="tel"
                  {...register('libraryPhone')}
                  error={errors.libraryPhone?.message}
                />
                <Input
                  label="Email Address"
                  type="email"
                  {...register('libraryEmail')}
                  error={errors.libraryEmail?.message}
                />
              </div>

              <Input
                label="Website"
                type="url"
                {...register('libraryWebsite')}
                error={errors.libraryWebsite?.message}
              />

              <div className="grid grid-cols-3 gap-4">
                <Input
                  label="Max Books per Member"
                  type="number"
                  min="1"
                  {...register('maxBooksPerMember', { valueAsNumber: true })}
                  error={errors.maxBooksPerMember?.message}
                />
                <Input
                  label="Default Loan Days"
                  type="number"
                  min="1"
                  {...register('defaultLoanDays', { valueAsNumber: true })}
                  error={errors.defaultLoanDays?.message}
                />
                <Input
                  label="Max Renewals"
                  type="number"
                  min="0"
                  {...register('maxRenewals', { valueAsNumber: true })}
                  error={errors.maxRenewals?.message}
                />
              </div>

              <div className="grid grid-cols-3 gap-4">
                <Input
                  label="Daily Fine Amount"
                  type="number"
                  step="0.01"
                  min="0"
                  {...register('dailyFineAmount', { valueAsNumber: true })}
                  error={errors.dailyFineAmount?.message}
                />
                <Input
                  label="Max Fine Limit"
                  type="number"
                  step="0.01"
                  min="0"
                  {...register('maxFineLimit', { valueAsNumber: true })}
                  error={errors.maxFineLimit?.message}
                />
                <Input
                  label="Reservation Expiry Days"
                  type="number"
                  min="1"
                  {...register('reservationExpiryDays', { valueAsNumber: true })}
                  error={errors.reservationExpiryDays?.message}
                />
              </div>

              <div className="space-y-4">
                <div className="flex items-center">
                  <input
                    id="allowReservations"
                    type="checkbox"
                    className="h-4 w-4 rounded border-gray-300 text-primary-600 focus:ring-primary-600"
                    {...register('allowReservations')}
                  />
                  <label htmlFor="allowReservations" className="ml-2 block text-sm text-gray-900">
                    Allow book reservations
                  </label>
                </div>

                <div className="flex items-center">
                  <input
                    id="requireReviewApproval"
                    type="checkbox"
                    className="h-4 w-4 rounded border-gray-300 text-primary-600 focus:ring-primary-600"
                    {...register('requireReviewApproval')}
                  />
                  <label htmlFor="requireReviewApproval" className="ml-2 block text-sm text-gray-900">
                    Require review approval before publishing
                  </label>
                </div>
              </div>

              <div className="flex justify-end pt-6 border-t">
                <Button
                  type="submit"
                  loading={updateLibrarySettingsMutation.isPending}
                >
                  Save Changes
                </Button>
              </div>
            </form>
          </CardContent>
        </Card>
      )}

      {/* Fine Settings */}
      {activeTab === 'fines' && (
        <Card>
          <CardHeader>
            <CardTitle>Fine Configuration</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="space-y-6">
              <div className="grid grid-cols-2 gap-4">
                <Input
                  label="Daily Fine Amount"
                  type="number"
                  step="0.01"
                  defaultValue={fineSettings?.dailyFineAmount || 0}
                />
                <Input
                  label="Maximum Fine Limit"
                  type="number"
                  step="0.01"
                  defaultValue={fineSettings?.maxFineLimit || 0}
                />
              </div>

              <div className="grid grid-cols-2 gap-4">
                <Input
                  label="Reminder Frequency (Days)"
                  type="number"
                  defaultValue={fineSettings?.reminderFrequencyDays || 7}
                />
                <div className="space-y-4">
                  <div className="flex items-center">
                    <input
                      id="allowFineWaiver"
                      type="checkbox"
                      className="h-4 w-4 rounded border-gray-300 text-primary-600 focus:ring-primary-600"
                      defaultChecked={fineSettings?.allowFineWaiver}
                    />
                    <label htmlFor="allowFineWaiver" className="ml-2 block text-sm text-gray-900">
                      Allow fine waiver
                    </label>
                  </div>

                  <div className="flex items-center">
                    <input
                      id="sendFineReminders"
                      type="checkbox"
                      className="h-4 w-4 rounded border-gray-300 text-primary-600 focus:ring-primary-600"
                      defaultChecked={fineSettings?.sendFineReminders}
                    />
                    <label htmlFor="sendFineReminders" className="ml-2 block text-sm text-gray-900">
                      Send fine reminders
                    </label>
                  </div>

                  <div className="flex items-center">
                    <input
                      id="blockMembershipOnMaxFine"
                      type="checkbox"
                      className="h-4 w-4 rounded border-gray-300 text-primary-600 focus:ring-primary-600"
                      defaultChecked={fineSettings?.blockMembershipOnMaxFine}
                    />
                    <label htmlFor="blockMembershipOnMaxFine" className="ml-2 block text-sm text-gray-900">
                      Block membership when max fine reached
                    </label>
                  </div>
                </div>
              </div>

              <div className="flex justify-end pt-6 border-t">
                <Button>Save Fine Settings</Button>
              </div>
            </div>
          </CardContent>
        </Card>
      )}

      {/* Notification Settings */}
      {activeTab === 'notifications' && (
        <Card>
          <CardHeader>
            <CardTitle>Notification Configuration</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="space-y-6">
              <div className="space-y-4">
                <h3 className="text-lg font-medium text-gray-900">Notification Types</h3>
                
                <div className="grid grid-cols-2 gap-4">
                  <div className="flex items-center">
                    <input
                      id="emailNotifications"
                      type="checkbox"
                      className="h-4 w-4 rounded border-gray-300 text-primary-600 focus:ring-primary-600"
                      defaultChecked={notificationSettings?.emailNotifications}
                    />
                    <label htmlFor="emailNotifications" className="ml-2 block text-sm text-gray-900">
                      Email notifications
                    </label>
                  </div>

                  <div className="flex items-center">
                    <input
                      id="pushNotifications"
                      type="checkbox"
                      className="h-4 w-4 rounded border-gray-300 text-primary-600 focus:ring-primary-600"
                      defaultChecked={notificationSettings?.pushNotifications}
                    />
                    <label htmlFor="pushNotifications" className="ml-2 block text-sm text-gray-900">
                      Push notifications
                    </label>
                  </div>

                  <div className="flex items-center">
                    <input
                      id="overdueReminders"
                      type="checkbox"
                      className="h-4 w-4 rounded border-gray-300 text-primary-600 focus:ring-primary-600"
                      defaultChecked={notificationSettings?.overdueReminders}
                    />
                    <label htmlFor="overdueReminders" className="ml-2 block text-sm text-gray-900">
                      Overdue reminders
                    </label>
                  </div>

                  <div className="flex items-center">
                    <input
                      id="fineReminders"
                      type="checkbox"
                      className="h-4 w-4 rounded border-gray-300 text-primary-600 focus:ring-primary-600"
                      defaultChecked={notificationSettings?.fineReminders}
                    />
                    <label htmlFor="fineReminders" className="ml-2 block text-sm text-gray-900">
                      Fine reminders
                    </label>
                  </div>

                  <div className="flex items-center">
                    <input
                      id="reservationReminders"
                      type="checkbox"
                      className="h-4 w-4 rounded border-gray-300 text-primary-600 focus:ring-primary-600"
                      defaultChecked={notificationSettings?.reservationReminders}
                    />
                    <label htmlFor="reservationReminders" className="ml-2 block text-sm text-gray-900">
                      Reservation reminders
                    </label>
                  </div>

                  <div className="flex items-center">
                    <input
                      id="membershipReminders"
                      type="checkbox"
                      className="h-4 w-4 rounded border-gray-300 text-primary-600 focus:ring-primary-600"
                      defaultChecked={notificationSettings?.membershipReminders}
                    />
                    <label htmlFor="membershipReminders" className="ml-2 block text-sm text-gray-900">
                      Membership reminders
                    </label>
                  </div>
                </div>
              </div>

              <div>
                <h3 className="text-lg font-medium text-gray-900 mb-4">Email Configuration</h3>
                <div className="grid grid-cols-2 gap-4">
                  <Input
                    label="SMTP Host"
                    defaultValue={notificationSettings?.smtpHost || ''}
                  />
                  <Input
                    label="SMTP Port"
                    type="number"
                    defaultValue={notificationSettings?.smtpPort || 587}
                  />
                  <Input
                    label="SMTP Username"
                    defaultValue={notificationSettings?.smtpUsername || ''}
                  />
                  <Input
                    label="SMTP Password"
                    type="password"
                    defaultValue={notificationSettings?.smtpPassword || ''}
                  />
                </div>
                
                <div className="mt-4">
                  <div className="flex items-center">
                    <input
                      id="smtpUseSSL"
                      type="checkbox"
                      className="h-4 w-4 rounded border-gray-300 text-primary-600 focus:ring-primary-600"
                      defaultChecked={notificationSettings?.smtpUseSSL}
                    />
                    <label htmlFor="smtpUseSSL" className="ml-2 block text-sm text-gray-900">
                      Use SSL/TLS
                    </label>
                  </div>
                </div>
              </div>

              <div className="flex justify-end pt-6 border-t">
                <Button>Save Notification Settings</Button>
              </div>
            </div>
          </CardContent>
        </Card>
      )}
    </div>
  )
}