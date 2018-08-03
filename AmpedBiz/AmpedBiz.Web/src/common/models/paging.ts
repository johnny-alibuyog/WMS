import { transient } from 'aurelia-framework';
import { Dictionary } from '../custom_types/dictionary';
import { appConfig } from '../../app-config';

@transient()
export class Filter implements Dictionary<any> {
  [key: string]: any;

  public onFilter: () => void;

  doFilter(): void {
    if (!this.onFilter)
      return;

    this.onFilter();
  }
}

@transient()
export class Sorter implements Dictionary<any> {
  [key: string]: any;

  public onSort: () => void;

  doSort(field: string): void {
    if (!this.onSort)
      return;

    /*
    switch (this[field]) {
      case SortDirection.None:
        this[field] = SortDirection.Ascending;
        break;
      case SortDirection.Ascending:
        this[field] = SortDirection.Descending;
        break;
      case SortDirection.Descending:
        this[field] = SortDirection.None;
        break;
      default:
        this[field] = SortDirection.Ascending;
        break;
    }
    */

    // just sort only one field for now
    if (this[field] == SortDirection.Ascending) {
      this[field] = SortDirection.Descending;
    }
    else {
      this[field] = SortDirection.Ascending;
    }

    for (var key in this) {
      if (key === field)
        continue;

      if (typeof this[key] === 'function')
        continue;

      this[key] = SortDirection.None;
    }

    this.onSort();
  }

  class(direction: SortDirection): string {
    switch (direction) {
      case SortDirection.Ascending:
        return 'sort-asc';
      case SortDirection.Descending:
        return 'sort-desc';
      default:
        return 'sort';
    }
  }
}

@transient()
export class Pager<T> implements PagerRequest, PagerResponse<T> {
  public offset: number;
  public size: number;
  public count: number;
  public items: T[];

  public get start(): number {
    return ((this.offset - 1) * this.size);
  }

  public get end(): number {
    return (this.start + this.size);
  }

  public onPage: () => void;

  constructor() {
    this.offset = 1;
    this.size = appConfig.page.itemsPerPage;
  }

  doPage(event: CustomEvent) {
    var pageNumber = <number>event.detail.pageNumber;
    if (this.offset === pageNumber)
      return;

    this.offset = pageNumber;

    if (!this.onPage)
      return;

    this.onPage();
  }
}

export interface PageRequest {
  sorter: Sorter;
  filter: Filter;
  pager: PagerRequest;
}

export interface PagerRequest {
  offset: number;
  size: number;
}

export interface PagerResponse<T> {
  count: number;
  items: T[];
}

export enum SortDirection {
  None = 0,
  Ascending = 1,
  Descending = 2
}

export type BuildPageRequestFn<T> = () => PageRequest; 