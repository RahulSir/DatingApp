export interface Pagination {
  currentPage: number;
  totalPages: number;
  itemsPerPage: number;
  totalItems: number;
}

export class PaginatedResult<T> {
  pagination: Pagination;
  result: T;
}
