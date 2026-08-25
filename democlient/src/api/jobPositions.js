import apiClient, { unwrap } from "./client";

const BASE = "/job-position";

export async function getJobPositions({ educationLevelId, keyword } = {}) {
  const res = await apiClient.get(BASE, {
    params: { educationLevelId: educationLevelId || undefined, keyword: keyword || undefined },
  });
  return unwrap(res) || [];
}

export async function getJobPositionDetail(id) {
  const res = await apiClient.get(`${BASE}/${id}`);
  return unwrap(res);
}

export async function createJobPosition(meta) {
  const res = await apiClient.post(BASE, meta);
  return unwrap(res);
}

export async function updateJobPosition(id, meta) {
  const res = await apiClient.put(`${BASE}/${id}`, meta);
  return unwrap(res);
}

export async function deleteJobPosition(id) {
  const res = await apiClient.delete(`${BASE}/${id}`);
  return unwrap(res);
}
