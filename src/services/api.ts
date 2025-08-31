import axios, { AxiosResponse } from 'axios'
import { useAuthStore } from '@/stores/authStore'
import toast from 'react-hot-toast'
import type {
  AuthResponse,
  LoginRequest,
  RegisterRequest,
  Book,
  Member,
  Transaction,
  Fine,
  Reservation,
  Notification,
  Review,
  Librarian,
  Category,
  DashboardOverview,
  PagedResult,
  SearchResult,
  StatisticsOverview,
  Alert,
  LibrarySettings,
  Report,
  ImportJob,
  ExportJob,
} from '@/types'

const API_BASE_URL = '/api'

// Create axios instance
const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
})

// Request interceptor to add auth token
api.interceptors.request.use((config) => {
  const token = useAuthStore.getState().token
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

// Response interceptor for error handling
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      useAuthStore.getState().logout()
      toast.error('Session expired. Please login again.')
    } else if (error.response?.status >= 500) {
      toast.error('Server error. Please try again later.')
    }
    return Promise.reject(error)
  }
)

// Auth API
export const authApi = {
  login: async (data: LoginRequest): Promise<AuthResponse> => {
    const response: AxiosResponse<AuthResponse> = await api.post('/auth/login', data)
    return response.data
  },

  register: async (data: RegisterRequest): Promise<AuthResponse> => {
    const response: AxiosResponse<AuthResponse> = await api.post('/auth/register', data)
    return response.data
  },

  refreshToken: async (refreshToken: string): Promise<AuthResponse> => {
    const response: AxiosResponse<AuthResponse> = await api.post('/auth/refresh', { refreshToken })
    return response.data
  },

  logout: async (): Promise<void> => {
    await api.post('/auth/logout')
  },

  getProfile: async (): Promise<any> => {
    const response = await api.get('/auth/profile')
    return response.data
  },

  updateProfile: async (data: any): Promise<void> => {
    await api.put('/auth/profile', data)
  },

  changePassword: async (data: any): Promise<void> => {
    await api.post('/auth/change-password', data)
  },
}

// Books API
export const booksApi = {
  getAll: async (params?: any): Promise<{ items: Book[]; totalCount: number }> => {
    const response = await api.get('/books', { params })
    return response.data
  },

  getById: async (id: number): Promise<Book> => {
    const response = await api.get(`/books/${id}`)
    return response.data
  },

  create: async (data: Partial<Book>): Promise<Book> => {
    const response = await api.post('/books', data)
    return response.data
  },

  update: async (id: number, data: Partial<Book>): Promise<void> => {
    await api.put(`/books/${id}`, data)
  },

  delete: async (id: number): Promise<void> => {
    await api.delete(`/books/${id}`)
  },
}

// Members API
export const membersApi = {
  getAll: async (params?: any): Promise<{ items: Member[]; totalCount: number }> => {
    const response = await api.get('/members', { params })
    return response.data
  },

  getById: async (id: number): Promise<Member> => {
    const response = await api.get(`/members/${id}`)
    return response.data
  },

  create: async (data: Partial<Member>): Promise<Member> => {
    const response = await api.post('/members', data)
    return response.data
  },

  update: async (id: number, data: Partial<Member>): Promise<void> => {
    await api.put(`/members/${id}`, data)
  },

  delete: async (id: number): Promise<void> => {
    await api.delete(`/members/${id}`)
  },

  extendMembership: async (id: number, data: { newEndDate: string; reason?: string }): Promise<void> => {
    await api.post(`/members/extend-membership/${id}`, data)
  },
}

// Categories API
export const categoriesApi = {
  getAll: async (params?: any): Promise<{ items: Category[]; totalCount: number }> => {
    const response = await api.get('/categories', { params })
    return response.data
  },

  getById: async (id: number): Promise<Category> => {
    const response = await api.get(`/categories/${id}`)
    return response.data
  },

  create: async (data: Partial<Category>): Promise<Category> => {
    const response = await api.post('/categories', data)
    return response.data
  },

  update: async (id: number, data: Partial<Category>): Promise<void> => {
    await api.put(`/categories/${id}`, data)
  },

  delete: async (id: number): Promise<void> => {
    await api.delete(`/categories/${id}`)
  },
}

// Transactions API
export const transactionsApi = {
  borrowBook: async (data: { bookId: number; memberId: number; days?: number; notes?: string }): Promise<Transaction> => {
    const response = await api.post('/transactions/borrow', data)
    return response.data
  },

  returnBook: async (data: { transactionId: number; notes?: string }): Promise<Transaction> => {
    const response = await api.post('/transactions/return', data)
    return response.data
  },

  renewBook: async (data: { transactionId: number; additionalDays?: number; notes?: string }): Promise<Transaction> => {
    const response = await api.post('/transactions/renew', data)
    return response.data
  },

  getById: async (id: number): Promise<Transaction> => {
    const response = await api.get(`/transactions/${id}`)
    return response.data
  },

  getByMember: async (memberId: number, params?: any): Promise<PagedResult<Transaction>> => {
    const response = await api.get(`/transactions/member/${memberId}`, { params })
    return response.data
  },

  getByBook: async (bookId: number, params?: any): Promise<PagedResult<Transaction>> => {
    const response = await api.get(`/transactions/book/${bookId}`, { params })
    return response.data
  },

  getOverdue: async (params?: any): Promise<PagedResult<Transaction>> => {
    const response = await api.get('/transactions/overdue', { params })
    return response.data
  },

  getActive: async (params?: any): Promise<PagedResult<Transaction>> => {
    const response = await api.get('/transactions/active', { params })
    return response.data
  },
}

// Fines API
export const finesApi = {
  getById: async (id: number): Promise<Fine> => {
    const response = await api.get(`/fines/${id}`)
    return response.data
  },

  getByMember: async (memberId: number, params?: any): Promise<PagedResult<Fine>> => {
    const response = await api.get(`/fines/member/${memberId}`, { params })
    return response.data
  },

  getPending: async (params?: any): Promise<PagedResult<Fine>> => {
    const response = await api.get('/fines/pending', { params })
    return response.data
  },

  getOverdue: async (params?: any): Promise<PagedResult<Fine>> => {
    const response = await api.get('/fines/overdue', { params })
    return response.data
  },

  payFine: async (data: { fineId: number; amount: number; paymentMethod: string; referenceNumber?: string; notes?: string }): Promise<Fine> => {
    const response = await api.post('/fines/pay', data)
    return response.data
  },

  waiveFine: async (data: { fineId: number; reason: string; notes?: string }): Promise<Fine> => {
    const response = await api.post('/fines/waive', data)
    return response.data
  },

  getMemberSummary: async (memberId: number): Promise<any> => {
    const response = await api.get(`/fines/summary/member/${memberId}`)
    return response.data
  },

  getOverallSummary: async (): Promise<any> => {
    const response = await api.get('/fines/summary/overall')
    return response.data
  },
}

// Reservations API
export const reservationsApi = {
  create: async (data: { bookId: number; memberId: number; priority?: number; notes?: string }): Promise<Reservation> => {
    const response = await api.post('/reservations', data)
    return response.data
  },

  cancel: async (data: { reservationId: number; reason?: string }): Promise<void> => {
    await api.post('/reservations/cancel', data)
  },

  getById: async (id: number): Promise<Reservation> => {
    const response = await api.get(`/reservations/${id}`)
    return response.data
  },

  getByMember: async (memberId: number, params?: any): Promise<PagedResult<Reservation>> => {
    const response = await api.get(`/reservations/member/${memberId}`, { params })
    return response.data
  },

  getByBook: async (bookId: number, params?: any): Promise<PagedResult<Reservation>> => {
    const response = await api.get(`/reservations/book/${bookId}`, { params })
    return response.data
  },

  getActive: async (params?: any): Promise<PagedResult<Reservation>> => {
    const response = await api.get('/reservations/active', { params })
    return response.data
  },

  getExpired: async (params?: any): Promise<PagedResult<Reservation>> => {
    const response = await api.get('/reservations/expired', { params })
    return response.data
  },

  fulfill: async (id: number): Promise<void> => {
    await api.post(`/reservations/fulfill/${id}`)
  },
}

// Dashboard API
export const dashboardApi = {
  getOverview: async (): Promise<DashboardOverview> => {
    const response = await api.get('/dashboard/overview')
    return response.data
  },

  getCirculationStats: async (params?: any): Promise<any> => {
    const response = await api.get('/dashboard/circulation-stats', { params })
    return response.data
  },

  getOverdueSummary: async (): Promise<any> => {
    const response = await api.get('/dashboard/overdue-summary')
    return response.data
  },

  getFineSummary: async (): Promise<any> => {
    const response = await api.get('/dashboard/fine-summary')
    return response.data
  },

  getMemberActivity: async (params?: any): Promise<any> => {
    const response = await api.get('/dashboard/member-activity', { params })
    return response.data
  },

  getBookPopularity: async (params?: any): Promise<any> => {
    const response = await api.get('/dashboard/book-popularity', { params })
    return response.data
  },

  getCategoryStats: async (): Promise<any> => {
    const response = await api.get('/dashboard/category-stats')
    return response.data
  },

  getSystemHealth: async (): Promise<any> => {
    const response = await api.get('/dashboard/system-health')
    return response.data
  },

  getTrends: async (params?: any): Promise<any> => {
    const response = await api.get('/dashboard/trends', { params })
    return response.data
  },
}

// Search API
export const searchApi = {
  globalSearch: async (query: string, params?: any): Promise<SearchResult> => {
    const response = await api.get('/search/global', { params: { query, ...params } })
    return response.data
  },

  searchBooks: async (query: string, params?: any): Promise<any> => {
    const response = await api.get('/search/books', { params: { query, ...params } })
    return response.data
  },

  searchMembers: async (query: string, params?: any): Promise<any> => {
    const response = await api.get('/search/members', { params: { query, ...params } })
    return response.data
  },

  advancedSearch: async (params: any): Promise<any> => {
    const response = await api.get('/search/advanced', { params })
    return response.data
  },

  getSuggestions: async (query: string, params?: any): Promise<string[]> => {
    const response = await api.get('/search/suggestions', { params: { query, ...params } })
    return response.data
  },

  getPopularSearches: async (params?: any): Promise<any[]> => {
    const response = await api.get('/search/popular-searches', { params })
    return response.data
  },

  saveSearch: async (data: any): Promise<void> => {
    await api.post('/search/save', data)
  },

  getSavedSearches: async (): Promise<any[]> => {
    const response = await api.get('/search/saved')
    return response.data
  },
}

// Statistics API
export const statisticsApi = {
  getOverview: async (): Promise<StatisticsOverview> => {
    const response = await api.get('/statistics/overview')
    return response.data
  },

  getCirculationStatistics: async (params?: any): Promise<any> => {
    const response = await api.get('/statistics/circulation', { params })
    return response.data
  },

  getBookStatistics: async (params?: any): Promise<any> => {
    const response = await api.get('/statistics/books', { params })
    return response.data
  },

  getMemberStatistics: async (params?: any): Promise<any> => {
    const response = await api.get('/statistics/members', { params })
    return response.data
  },

  getFineStatistics: async (params?: any): Promise<any> => {
    const response = await api.get('/statistics/fines', { params })
    return response.data
  },

  getCategoryStatistics: async (params?: any): Promise<any> => {
    const response = await api.get('/statistics/categories', { params })
    return response.data
  },

  getLibrarianStatistics: async (params?: any): Promise<any> => {
    const response = await api.get('/statistics/librarians', { params })
    return response.data
  },

  getTrends: async (params?: any): Promise<any> => {
    const response = await api.get('/statistics/trends', { params })
    return response.data
  },

  getTopBooks: async (params?: any): Promise<any> => {
    const response = await api.get('/statistics/top-books', { params })
    return response.data
  },

  getTopMembers: async (params?: any): Promise<any> => {
    const response = await api.get('/statistics/top-members', { params })
    return response.data
  },
}

// Notifications API
export const notificationsApi = {
  create: async (data: any): Promise<Notification> => {
    const response = await api.post('/notifications', data)
    return response.data
  },

  sendBulk: async (data: any): Promise<any> => {
    const response = await api.post('/notifications/bulk', data)
    return response.data
  },

  getById: async (id: number): Promise<Notification> => {
    const response = await api.get(`/notifications/${id}`)
    return response.data
  },

  getByMember: async (memberId: number, params?: any): Promise<PagedResult<Notification>> => {
    const response = await api.get(`/notifications/member/${memberId}`, { params })
    return response.data
  },

  getUnreadByMember: async (memberId: number, params?: any): Promise<PagedResult<Notification>> => {
    const response = await api.get(`/notifications/unread/member/${memberId}`, { params })
    return response.data
  },

  markAsRead: async (id: number): Promise<void> => {
    await api.post(`/notifications/mark-read/${id}`)
  },

  markMultipleAsRead: async (ids: number[]): Promise<void> => {
    await api.post('/notifications/mark-read-bulk', { notificationIds: ids })
  },

  delete: async (id: number): Promise<void> => {
    await api.delete(`/notifications/${id}`)
  },
}

// Reviews API
export const reviewsApi = {
  create: async (data: any): Promise<Review> => {
    const response = await api.post('/reviews', data)
    return response.data
  },

  update: async (id: number, data: any): Promise<Review> => {
    const response = await api.put(`/reviews/${id}`, data)
    return response.data
  },

  delete: async (id: number): Promise<void> => {
    await api.delete(`/reviews/${id}`)
  },

  getById: async (id: number): Promise<Review> => {
    const response = await api.get(`/reviews/${id}`)
    return response.data
  },

  getByBook: async (bookId: number, params?: any): Promise<PagedResult<Review>> => {
    const response = await api.get(`/reviews/book/${bookId}`, { params })
    return response.data
  },

  getByMember: async (memberId: number, params?: any): Promise<PagedResult<Review>> => {
    const response = await api.get(`/reviews/member/${memberId}`, { params })
    return response.data
  },

  getApproved: async (params?: any): Promise<PagedResult<Review>> => {
    const response = await api.get('/reviews/approved', { params })
    return response.data
  },

  getPending: async (params?: any): Promise<PagedResult<Review>> => {
    const response = await api.get('/reviews/pending', { params })
    return response.data
  },

  approve: async (id: number): Promise<void> => {
    await api.post(`/reviews/approve/${id}`)
  },

  reject: async (id: number, data: { reason: string; notes?: string }): Promise<void> => {
    await api.post(`/reviews/reject/${id}`, data)
  },
}

// Librarians API
export const librariansApi = {
  getById: async (id: number): Promise<Librarian> => {
    const response = await api.get(`/librarians/${id}`)
    return response.data
  },

  getList: async (params?: any): Promise<PagedResult<Librarian>> => {
    const response = await api.get('/librarians', { params })
    return response.data
  },

  create: async (data: any): Promise<Librarian> => {
    const response = await api.post('/librarians', data)
    return response.data
  },

  update: async (id: number, data: any): Promise<Librarian> => {
    const response = await api.put(`/librarians/${id}`, data)
    return response.data
  },

  delete: async (id: number): Promise<void> => {
    await api.delete(`/librarians/${id}`)
  },

  activate: async (id: number): Promise<void> => {
    await api.post(`/librarians/activate/${id}`)
  },

  deactivate: async (id: number): Promise<void> => {
    await api.post(`/librarians/deactivate/${id}`)
  },

  changeRole: async (id: number, data: any): Promise<Librarian> => {
    const response = await api.post(`/librarians/change-role/${id}`, data)
    return response.data
  },

  getStats: async (): Promise<any> => {
    const response = await api.get('/librarians/stats')
    return response.data
  },

  getActivity: async (id: number, params?: any): Promise<PagedResult<any>> => {
    const response = await api.get(`/librarians/activity/${id}`, { params })
    return response.data
  },
}

// Alerts API
export const alertsApi = {
  getMyAlerts: async (): Promise<any> => {
    const response = await api.get('/alerts/my')
    return response.data
  },

  getOverdueAlerts: async (): Promise<any> => {
    const response = await api.get('/alerts/overdue')
    return response.data
  },

  getFineAlerts: async (): Promise<any> => {
    const response = await api.get('/alerts/fines')
    return response.data
  },

  getReservationAlerts: async (): Promise<any> => {
    const response = await api.get('/alerts/reservations')
    return response.data
  },

  getMembershipAlerts: async (): Promise<any> => {
    const response = await api.get('/alerts/membership')
    return response.data
  },

  getSummary: async (): Promise<any> => {
    const response = await api.get('/alerts/summary')
    return response.data
  },

  dismissAlert: async (alertId: number): Promise<void> => {
    await api.post(`/alerts/dismiss/${alertId}`)
  },

  dismissAllAlerts: async (): Promise<void> => {
    await api.post('/alerts/dismiss-all')
  },

  getSettings: async (): Promise<any> => {
    const response = await api.get('/alerts/settings')
    return response.data
  },

  updateSettings: async (data: any): Promise<any> => {
    const response = await api.put('/alerts/settings', data)
    return response.data
  },
}

// Settings API
export const settingsApi = {
  getAll: async (): Promise<any[]> => {
    const response = await api.get('/settings')
    return response.data
  },

  getByKey: async (key: string): Promise<any> => {
    const response = await api.get(`/settings/${key}`)
    return response.data
  },

  update: async (key: string, data: any): Promise<any> => {
    const response = await api.put(`/settings/${key}`, data)
    return response.data
  },

  updateBulk: async (data: any): Promise<any[]> => {
    const response = await api.post('/settings/bulk', data)
    return response.data
  },

  getLibrarySettings: async (): Promise<LibrarySettings> => {
    const response = await api.get('/settings/library')
    return response.data
  },

  updateLibrarySettings: async (data: LibrarySettings): Promise<LibrarySettings> => {
    const response = await api.put('/settings/library', data)
    return response.data
  },

  getFineSettings: async (): Promise<any> => {
    const response = await api.get('/settings/fines')
    return response.data
  },

  updateFineSettings: async (data: any): Promise<any> => {
    const response = await api.put('/settings/fines', data)
    return response.data
  },

  getNotificationSettings: async (): Promise<any> => {
    const response = await api.get('/settings/notifications')
    return response.data
  },

  updateNotificationSettings: async (data: any): Promise<any> => {
    const response = await api.put('/settings/notifications', data)
    return response.data
  },
}

// Reports API
export const reportsApi = {
  generate: async (data: any): Promise<Report> => {
    const response = await api.post('/reports/generate', data)
    return response.data
  },

  getById: async (id: number): Promise<Report> => {
    const response = await api.get(`/reports/${id}`)
    return response.data
  },

  download: async (id: number): Promise<Blob> => {
    const response = await api.get(`/reports/download/${id}`, { responseType: 'blob' })
    return response.data
  },

  getList: async (params?: any): Promise<PagedResult<Report>> => {
    const response = await api.get('/reports/list', { params })
    return response.data
  },

  getAvailableTypes: async (): Promise<any[]> => {
    const response = await api.get('/reports/types')
    return response.data
  },

  schedule: async (data: any): Promise<Report> => {
    const response = await api.post('/reports/schedule', data)
    return response.data
  },

  delete: async (id: number): Promise<void> => {
    await api.delete(`/reports/${id}`)
  },
}

// Import/Export API
export const importExportApi = {
  import: async (data: FormData): Promise<any> => {
    const response = await api.post('/import-export/import', data, {
      headers: { 'Content-Type': 'multipart/form-data' }
    })
    return response.data
  },

  export: async (data: any): Promise<Blob> => {
    const response = await api.post('/import-export/export', data, { responseType: 'blob' })
    return response.data
  },

  getTemplates: async (): Promise<any[]> => {
    const response = await api.get('/import-export/templates')
    return response.data
  },

  downloadTemplate: async (id: number): Promise<Blob> => {
    const response = await api.get(`/import-export/templates/${id}/download`, { responseType: 'blob' })
    return response.data
  },

  getImportHistory: async (params?: any): Promise<PagedResult<ImportJob>> => {
    const response = await api.get('/import-export/import-history', { params })
    return response.data
  },

  getExportHistory: async (params?: any): Promise<PagedResult<ExportJob>> => {
    const response = await api.get('/import-export/export-history', { params })
    return response.data
  },

  getSupportedFormats: async (): Promise<any> => {
    const response = await api.get('/import-export/supported-formats')
    return response.data
  },

  validateData: async (data: FormData): Promise<any> => {
    const response = await api.post('/import-export/validate', data, {
      headers: { 'Content-Type': 'multipart/form-data' }
    })
    return response.data
  },
}

// Audit API
export const auditApi = {
  getAuditLogs: async (params?: any): Promise<PagedResult<any>> => {
    const response = await api.get('/audit', { params })
    return response.data
  },

  getById: async (id: number): Promise<any> => {
    const response = await api.get(`/audit/${id}`)
    return response.data
  },

  getByEntity: async (entityType: string, entityId: number, params?: any): Promise<PagedResult<any>> => {
    const response = await api.get(`/audit/entity/${entityType}/${entityId}`, { params })
    return response.data
  },

  getByUser: async (userId: number, params?: any): Promise<PagedResult<any>> => {
    const response = await api.get(`/audit/user/${userId}`, { params })
    return response.data
  },

  getAvailableActions: async (): Promise<string[]> => {
    const response = await api.get('/audit/actions')
    return response.data
  },

  getAvailableEntityTypes: async (): Promise<string[]> => {
    const response = await api.get('/audit/entity-types')
    return response.data
  },

  getSummary: async (params?: any): Promise<any> => {
    const response = await api.get('/audit/summary', { params })
    return response.data
  },

  exportAuditLogs: async (params?: any): Promise<Blob> => {
    const response = await api.get('/audit/export', { params, responseType: 'blob' })
    return response.data
  },
}

// Health API
export const healthApi = {
  getHealth: async (): Promise<any> => {
    const response = await api.get('/health')
    return response.data
  },

  getDetailedHealth: async (): Promise<any> => {
    const response = await api.get('/health/detailed')
    return response.data
  },

  getReadiness: async (): Promise<any> => {
    const response = await api.get('/health/ready')
    return response.data
  },

  getLiveness: async (): Promise<any> => {
    const response = await api.get('/health/live')
    return response.data
  },

  getDatabaseHealth: async (): Promise<any> => {
    const response = await api.get('/health/database')
    return response.data
  },

  getStorageHealth: async (): Promise<any> => {
    const response = await api.get('/health/storage')
    return response.data
  },
}

export default api