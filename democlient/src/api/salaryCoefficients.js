import apiClient, { unwrap } from "./client";

const BASE = "/education-level-salary-coefficient";

export async function getSalaryCoefficients() {
  const res = await apiClient.get(BASE);
  return unwrap(res) || [];
}

export async function getSalaryCoefficientByEducationLevel(educationLevelId) {
  const res = await apiClient.get(`/education-levels/${educationLevelId}/salary-coefficient`);
  return unwrap(res) || [];
}

export async function upsertSalaryCoefficient(meta) {
  const res = await apiClient.post(BASE, meta);
  return unwrap(res);
}

export async function deleteSalaryCoefficient(id) {
  const res = await apiClient.delete(`${BASE}/${id}`);
  return unwrap(res);
}
