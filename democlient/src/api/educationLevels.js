import apiClient, { unwrap } from "./client";

const BASE = "/education-levels";

export async function getEducationLevels() {
  const res = await apiClient.get(BASE);
  return unwrap(res) || [];
}

export async function getEducationLevelDetail(id) {
  const res = await apiClient.get(`${BASE}/${id}`);
  return unwrap(res);
}

export async function getJobPositionsByEducationLevel(id) {
  const res = await apiClient.get(`${BASE}/${id}/job-positions`);
  return unwrap(res) || [];
}

export async function createEducationLevel(meta) {
  const res = await apiClient.post(BASE, meta);
  return unwrap(res);
}

export async function updateEducationLevel(id, meta) {
  const res = await apiClient.put(`${BASE}/${id}`, meta);
  return unwrap(res);
}

export async function deleteEducationLevel(id) {
  const res = await apiClient.delete(`${BASE}/${id}`);
  return unwrap(res);
}
