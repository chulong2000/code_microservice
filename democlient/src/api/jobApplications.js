import apiClient, { unwrap } from "./client";

const BASE = "/job-applications";

export async function getJobApplications({ keyword, jobPositionId, appliedFrom, appliedTo } = {}) {
  const res = await apiClient.get(BASE, {
    params: {
      Keyword: keyword || undefined,
      JobPositionId: jobPositionId || undefined,
      AppliedFrom: appliedFrom || undefined,
      AppliedTo: appliedTo || undefined,
    },
  });
  return unwrap(res) || [];
}

export async function getJobApplicationDetail(id) {
  const res = await apiClient.get(`${BASE}/${id}`);
  return unwrap(res);
}

export async function createJobApplication(meta) {
  const res = await apiClient.post(BASE, meta);
  return unwrap(res);
}

export async function deleteJobApplication(id) {
  const res = await apiClient.delete(`${BASE}/${id}`);
  return unwrap(res);
}
