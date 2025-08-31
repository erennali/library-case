import { Routes, Route, Navigate } from 'react-router-dom'
import { useAuthStore } from '@/stores/authStore'
import Layout from '@/components/Layout'
import LoginPage from '@/pages/auth/LoginPage'
import RegisterPage from '@/pages/auth/RegisterPage'
import DashboardPage from '@/pages/DashboardPage'
import BooksPage from '@/pages/books/BooksPage'
import BookDetailPage from '@/pages/books/BookDetailPage'
import MembersPage from '@/pages/members/MembersPage'
import MemberDetailPage from '@/pages/members/MemberDetailPage'
import TransactionsPage from '@/pages/transactions/TransactionsPage'
import ReservationsPage from '@/pages/reservations/ReservationsPage'
import FinesPage from '@/pages/fines/FinesPage'
import ReportsPage from '@/pages/reports/ReportsPage'
import SettingsPage from '@/pages/settings/SettingsPage'
import NotificationsPage from '@/pages/notifications/NotificationsPage'
import SearchPage from '@/pages/search/SearchPage'
import StatisticsPage from '@/pages/statistics/StatisticsPage'
import AuditPage from '@/pages/audit/AuditPage'
import LibrariansPage from '@/pages/librarians/LibrariansPage'
import CategoriesPage from '@/pages/categories/CategoriesPage'
import ReviewsPage from '@/pages/reviews/ReviewsPage'
import AlertsPage from '@/pages/alerts/AlertsPage'
import ImportExportPage from '@/pages/import-export/ImportExportPage'

function App() {
  const { isAuthenticated } = useAuthStore()

  if (!isAuthenticated) {
    return (
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="*" element={<Navigate to="/login" replace />} />
      </Routes>
    )
  }

  return (
    <Layout>
      <Routes>
        <Route path="/" element={<Navigate to="/dashboard" replace />} />
        <Route path="/dashboard" element={<DashboardPage />} />
        <Route path="/books" element={<BooksPage />} />
        <Route path="/books/:id" element={<BookDetailPage />} />
        <Route path="/members" element={<MembersPage />} />
        <Route path="/members/:id" element={<MemberDetailPage />} />
        <Route path="/transactions" element={<TransactionsPage />} />
        <Route path="/reservations" element={<ReservationsPage />} />
        <Route path="/fines" element={<FinesPage />} />
        <Route path="/reports" element={<ReportsPage />} />
        <Route path="/settings" element={<SettingsPage />} />
        <Route path="/notifications" element={<NotificationsPage />} />
        <Route path="/search" element={<SearchPage />} />
        <Route path="/statistics" element={<StatisticsPage />} />
        <Route path="/audit" element={<AuditPage />} />
        <Route path="/librarians" element={<LibrariansPage />} />
        <Route path="/categories" element={<CategoriesPage />} />
        <Route path="/reviews" element={<ReviewsPage />} />
        <Route path="/alerts" element={<AlertsPage />} />
        <Route path="/import-export" element={<ImportExportPage />} />
        <Route path="*" element={<Navigate to="/dashboard" replace />} />
      </Routes>
    </Layout>
  )
}

export default App