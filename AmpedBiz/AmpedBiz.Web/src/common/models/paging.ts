import { transient } from 'aurelia-framework';
import { Dictionary } from '../custom_types/dictionary';
import { appConfig } from '../../app-config';

@transient()
export class Filter implements Dictionary<any> {
  [key: string]: any;

  public onFilter: () => void;

  public doFilter(): void {
    if (!this.onFilter)
      return;

    this.onFilter();
  }
}

@transient()
export class Sorter implements Dictionary<any> {
  [key: string]: any;

  public onSort: () => void;

  public doSort(field: string): void {
    if (!this.onSort)
      return;

    // just sort only one field for now
    for (let key in this) {
      if (typeof this[key] === 'function')
        continue;

      if (key === field) {
        this[key.toString()] = this[field] == SortDirection.Ascending
          ? SortDirection.Descending
          : SortDirection.Ascending
      }
      else {
        this[key.toString()] = SortDirection.None;
      }
    }

    this.onSort();
  }

  public class(direction: SortDirection): string {
    switch (direction) {
      case SortDirection.Ascending:
        return 'sort-up';
      case SortDirection.Descending:
        return 'sort-down';
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

  public doPage(event: CustomEvent): void {
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
