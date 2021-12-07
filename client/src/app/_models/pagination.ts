export interface Pagination{
    currentPage: number;
    itemsPerPage: number;
    totalItems:number;
    totalPages:number;

}
export class PaginatedResult<Type>{
    result: Type
    pagination: Pagination;

}