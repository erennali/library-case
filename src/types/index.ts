// Auth Types
export interface User {
  id: number
  email: string
  firstName: string
  lastName: string
  phoneNumber?: string
  userType: 'Member' | 'Librarian'
  status: string
  roles: string[]
  createdAt: string
  lastLoginAt?: string
}

export interface AuthResponse {
  success: boolean
  accessToken?: string
  refreshToken?: string
  message?: string
  user?: User
  expiresAt?: string
}

export interface LoginRequest {
  email: string
  password: string
  rememberMe?: boolean
}

export interface RegisterRequest {
  email: string
  password: string
  confirmPassword: string
  firstName: string
  lastName: string
  phoneNumber?: string
  userType: 'Member' | 'Librarian'
}

// Book Types
export interface Book {
  id: number
  isbn: string
  title: string
  author: string
  publisher?: string
  publicationDate?: string
  description?: string
  categoryId: number
  category?: Category
  totalCopies: number
  availableCopies: number
  language?: string
  pageCount: number
  imageUrl?: string
  price?: number
  status: BookStatus
  createdAt: string
  updatedAt: string
}

export enum BookStatus {
  Available = 'Available',
  Borrowed = 'Borrowed',
  Reserved = 'Reserved',
  OutOfStock = 'OutOfStock',
  Discontinued = 'Discontinued',
  UnderMaintenance = 'UnderMaintenance'
}

// Category Types
export interface Category {
  id: number
  name: string
  description?: string
  parentCategoryId?: number
  parentCategory?: Category
  isActive: boolean
  createdAt: string
  subCategories?: Category[]
  books?: Book[]
}

// Member Types
export interface Member {
  id: number
  membershipNumber: string
  firstName: string
  lastName: string
  email: string
  phoneNumber?: string
  address?: string
  dateOfBirth: string
  membershipType: MembershipType
  membershipStartDate: string
  membershipEndDate: string
  status: MemberStatus
  maxBooksAllowed: number
  currentBooksCount: number
  totalFinesOwed: number
  maxFineLimit: number
  createdAt: string
  updatedAt: string
}

export enum MembershipType {
  Student = 'Student',
  Faculty = 'Faculty',
  Staff = 'Staff',
  Regular = 'Regular',
  Premium = 'Premium'
}

export enum MemberStatus {
  Active = 'Active',
  Suspended = 'Suspended',
  Expired = 'Expired',
  Blocked = 'Blocked'
}

// Transaction Types
export interface Transaction {
  id: number
  transactionNumber: string
  bookId: number
  bookTitle: string
  memberId: number
  memberName: string
  type: TransactionType
  checkoutDate: string
  dueDate: string
  returnDate?: string
  status: TransactionStatus
  renewalCount?: number
  fineAmount?: number
  notes?: string
  processedByLibrarianId?: number
  processedByLibrarianName?: string
  createdAt: string
}

export enum TransactionType {
  Borrow = 'Borrow',
  Checkout = 'Checkout',
  Return = 'Return',
  Renewal = 'Renewal',
  Lost = 'Lost',
  Damaged = 'Damaged'
}

export enum TransactionStatus {
  Active = 'Active',
  Borrowed = 'Borrowed',
  Returned = 'Returned',
  Overdue = 'Overdue',
  Lost = 'Lost',
  Damaged = 'Damaged'
}

// Fine Types
export interface Fine {
  id: number
  fineNumber: string
  transactionId: number
  transactionNumber: string
  memberId: number
  memberName: string
  type: FineType
  amount: number
  issueDate: string
  dueDate?: string
  status: FineStatus
  paidDate?: string
  paidAmount?: number
  description?: string
  notes?: string
  processedByLibrarianId?: number
  processedByLibrarianName?: string
  createdAt: string
  updatedAt: string
}

export enum FineType {
  OverdueBook = 'OverdueBook',
  DamagedBook = 'DamagedBook',
  LostBook = 'LostBook',
  LateReturn = 'LateReturn',
  Other = 'Other'
}

export enum FineStatus {
  Pending = 'Pending',
  Unpaid = 'Unpaid',
  Overdue = 'Overdue',
  PartiallyPaid = 'PartiallyPaid',
  Paid = 'Paid',
  Waived = 'Waived',
  Cancelled = 'Cancelled'
}

// Reservation Types
export interface Reservation {
  id: number
  reservationNumber: string
  bookId: number
  bookTitle: string
  memberId: number
  memberName: string
  reservationDate: string
  expiryDate: string
  notifiedDate?: string
  status: ReservationStatus
  priority: number
  notes?: string
  fulfilledDate?: string
  processedByLibrarianId?: number
  processedByLibrarianName?: string
  createdAt: string
  updatedAt: string
}

export enum ReservationStatus {
  Active = 'Active',
  Fulfilled = 'Fulfilled',
  Expired = 'Expired',
  Cancelled = 'Cancelled',
  OnHold = 'OnHold'
}

// Notification Types
export interface Notification {
  id: number
  memberId: number
  memberName: string
  title: string
  message: string
  type: NotificationType
  status: NotificationStatus
  createdAt: string
  readAt?: string
  relatedEntityId?: number
  relatedEntityType?: string
  isEmailSent: boolean
  emailSentAt?: string
}

export enum NotificationType {
  BookDue = 'BookDue',
  BookOverdue = 'BookOverdue',
  BookAvailable = 'BookAvailable',
  ReservationExpiring = 'ReservationExpiring',
  FineIssued = 'FineIssued',
  AccountSuspended = 'AccountSuspended',
  General = 'General'
}

export enum NotificationStatus {
  Unread = 'Unread',
  Read = 'Read',
  Archived = 'Archived'
}

// Review Types
export interface Review {
  id: number
  bookId: number
  bookTitle: string
  memberId: number
  memberName: string
  rating: number
  comment?: string
  reviewDate: string
  isApproved: boolean
}

// Librarian Types
export interface Librarian {
  id: number
  employeeNumber: string
  firstName: string
  lastName: string
  email: string
  phoneNumber?: string
  role: LibrarianRole
  status: LibrarianStatus
  hireDate: string
  department?: string
  createdAt: string
  updatedAt: string
}

export enum LibrarianRole {
  Assistant = 'Assistant',
  Librarian = 'Librarian',
  SeniorLibrarian = 'SeniorLibrarian',
  HeadLibrarian = 'HeadLibrarian',
  Administrator = 'Administrator'
}

export enum LibrarianStatus {
  Active = 'Active',
  Inactive = 'Inactive',
  OnLeave = 'OnLeave',
  Terminated = 'Terminated'
}

// Dashboard Types
export interface DashboardOverview {
  totalBooks: number
  availableBooks: number
  borrowedBooks: number
  overdueBooks: number
  totalMembers: number
  activeMembers: number
  totalTransactions: number
  overdueTransactions: number
  totalFines: number
  activeReservations: number
  recentActivities: RecentActivity[]
  quickActions: QuickAction[]
}

export interface RecentActivity {
  id: number
  type: string
  description: string
  userName?: string
  timestamp: string
  status?: string
}

export interface QuickAction {
  name: string
  description: string
  action: string
  icon: string
  isEnabled: boolean
}

// Common Types
export interface PagedResult<T> {
  items: T[]
  totalCount: number
  page: number
  pageSize: number
}

export interface ApiError {
  message: string
  details?: string
  correlationId?: string
  timestamp: string
}

export interface FileResult {
  fileBytes: string
  contentType: string
  fileName: string
  fileSize: number
}

// Search Types
export interface SearchResult {
  query: string
  totalResults: number
  page: number
  pageSize: number
  results: SearchResultItem[]
}

export interface SearchResultItem {
  id: number
  type: string
  title: string
  description: string
  imageUrl?: string
  metadata: Record<string, any>
}

// Statistics Types
export interface StatisticsOverview {
  totalBooks: number
  totalMembers: number
  totalTransactions: number
  overdueItems: number
  totalFines: number
  activeReservations: number
  booksByCategory: Record<string, number>
  membersByType: Record<string, number>
}

// Alert Types
export interface Alert {
  id: number
  title: string
  message: string
  alertType: string
  severity: string
  isActive: boolean
  createdAt: string
  expiresAt?: string
  acknowledgedAt?: string
  acknowledgedByLibrarianName?: string
  additionalData?: string
  source?: string
  priority: number
}

// Settings Types
export interface LibrarySettings {
  libraryName: string
  libraryAddress: string
  libraryPhone: string
  libraryEmail: string
  libraryWebsite: string
  currency: string
  maxBooksPerMember: number
  defaultLoanDays: number
  maxRenewals: number
  dailyFineAmount: number
  maxFineLimit: number
  allowReservations: boolean
  reservationExpiryDays: number
  requireReviewApproval: boolean
}

// Report Types
export interface Report {
  id: number
  reportType: string
  format: string
  status: string
  filePath?: string
  fileUrl?: string
  fileSize?: number
  parameters?: Record<string, any>
  generatedAt?: string
  scheduledAt?: string
  createdAt: string
}

// Import/Export Types
export interface ImportJob {
  id: number
  fileName: string
  fileSize: number
  importType: string
  status: string
  totalRecords: number
  processedRecords: number
  successRecords: number
  failedRecords: number
  errors: string[]
  createdAt: string
  updatedAt: string
}

export interface ExportJob {
  id: number
  exportType: string
  format: string
  status: string
  totalRecords: number
  processedRecords: number
  fileName: string
  fileSize: number
  downloadUrl: string
  createdAt: string
  updatedAt: string
}