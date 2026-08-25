import axios from "axios";

const apiClient = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || "http://localhost:5083/api/v1",
  headers: {
    "Content-Type": "application/json",
  },
});

export function unwrap(response) {
  return response.data?.data;
}

export function errorMessage(error) {
  return (
    error.response?.data?.message ||
    error.response?.data?.title ||
    error.message ||
    "Đã xảy ra lỗi không xác định"
  );
}

export default apiClient;
