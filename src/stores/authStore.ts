import { create } from 'zustand'
import { persist } from 'zustand/middleware'
import { User, AuthResponse } from '@/types'
import { authApi } from '@/services/api'

interface AuthState {
  user: User | null
  token: string | null
  isAuthenticated: boolean
  isLoading: boolean
  login: (email: string, password: string) => Promise<AuthResponse>
  register: (data: any) => Promise<AuthResponse>
  logout: () => void
  refreshToken: () => Promise<void>
  updateProfile: (data: any) => Promise<void>
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set, get) => ({
      user: null,
      token: null,
      isAuthenticated: false,
      isLoading: false,

      login: async (email: string, password: string) => {
        set({ isLoading: true })
        try {
          const response = await authApi.login({ email, password })
          if (response.success && response.accessToken && response.user) {
            set({
              user: response.user,
              token: response.accessToken,
              isAuthenticated: true,
              isLoading: false,
            })
          }
          return response
        } catch (error) {
          set({ isLoading: false })
          throw error
        }
      },

      register: async (data) => {
        set({ isLoading: true })
        try {
          const response = await authApi.register(data)
          if (response.success && response.accessToken && response.user) {
            set({
              user: response.user,
              token: response.accessToken,
              isAuthenticated: true,
              isLoading: false,
            })
          }
          return response
        } catch (error) {
          set({ isLoading: false })
          throw error
        }
      },

      logout: () => {
        set({
          user: null,
          token: null,
          isAuthenticated: false,
          isLoading: false,
        })
      },

      refreshToken: async () => {
        // Implementation for token refresh
      },

      updateProfile: async (data) => {
        const { user } = get()
        if (user) {
          const updatedUser = { ...user, ...data }
          set({ user: updatedUser })
        }
      },
    }),
    {
      name: 'auth-storage',
      partialize: (state) => ({
        user: state.user,
        token: state.token,
        isAuthenticated: state.isAuthenticated,
      }),
    }
  )
)