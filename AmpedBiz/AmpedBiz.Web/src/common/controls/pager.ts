import {autoinject, bindable, bindingMode, customElement, computedFrom} from 'aurelia-framework'
import {LogManager} from 'aurelia-framework';
import {appConfig} from '../../app-config';
const logger = LogManager.getLogger('pager');

@autoinject
export class Pager {

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  //@bindable()
  public currentPage: number;

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  //@bindable()
  public totalItems: number;

  public totalPages: number;

  public pages: Page[] = [];

  private _element: Element;

  private _config: PageConfig = {};

  constructor(element: Element) {
    this._element = element;

    var defaultIfNull = (value: any, alternative: any) => value != null ? value : alternative;

    this._config = {
      maxSize: defaultIfNull(appConfig.page.maxSize, 5),
      itemsPerPage: defaultIfNull(appConfig.page.itemsPerPage, 10),
      boundaryLinks: defaultIfNull(appConfig.page.boundaryLinks, true),
      directionLinks: defaultIfNull(appConfig.page.directionLinks, true),
      firstText: defaultIfNull(appConfig.page.firstText, '<<'),
      previousText: defaultIfNull(appConfig.page.previousText, '<'),
      nextText: defaultIfNull(appConfig.page.nextText, '>'),
      lastText: defaultIfNull(appConfig.page.lastText, '>>'),
      rotate: defaultIfNull(appConfig.page.rotate, false)
    };
  }

  attached() {
    this.buildPages();
  }

  currentPageChanged() {
    this.buildPages();
  }
  
  totalItemsChanged(){
    this.buildPages();
  }

  calculateTotalPages(): number {
    var totalPages = this._config.itemsPerPage < 1 ? 1 : Math.ceil(this.totalItems / this._config.itemsPerPage);
    totalPages = Math.max(totalPages || 0, 1);

    if (this.currentPage > totalPages)
      this.currentPage = totalPages;

    return totalPages;
  }

  getText(key: string): string {
    return this._config[key + 'Text'];
  }

  get directionLinks(): boolean {
    return this._config.directionLinks;
  }

  get boundaryLinks(): boolean {
    return this._config.boundaryLinks;
  }

  @computedFrom('currentPage')
  get noPrevious(): boolean {
    return this.currentPage === 1;
  }

  @computedFrom('currentPage', 'totalPage')
  get noNext(): boolean {
    return this.currentPage === this.totalPages;
  }

  selectPage(pageNumber: number) {
    if (pageNumber < 1)
      return;

    if (pageNumber > this.totalPages)
      return;

    this.currentPage = pageNumber;
    this.buildPages();

    var changeEvent = document.createEvent('CustomEvent');
    changeEvent.initCustomEvent('change', true, true, { pageNumber: pageNumber });
    this._element.dispatchEvent(changeEvent);
  }

  buildPages(): void {
    // Clear pages
    this.pages = [];

    this.totalPages = this.calculateTotalPages();

    // Default page limits
    var startPage = 1;
    var endPage = this.totalPages;
    var isMaxSized = this._config.maxSize < this.totalPages;

    // recompute if maxSize
    if (isMaxSized) {
      if (this._config.rotate) {
        // Current page is displayed in the middle of the visible ones
        startPage = Math.max(this.currentPage - Math.floor(this._config.maxSize / 2), 1);
        endPage = startPage + this._config.maxSize - 1;

        // Adjust if limit is exceeded
        if (endPage > this.totalPages) {
          endPage = this.totalPages;
          startPage = endPage - this._config.maxSize + 1;
        }
      }
      else {
        // Visible pages are paginated with maxSize
        startPage = ((Math.ceil(this.currentPage / this._config.maxSize) - 1) * this._config.maxSize) + 1;

        // Adjust last page if limit is exceeded
        endPage = Math.min(startPage + this._config.maxSize - 1, this.totalPages);
      }
    }

    // Add page number links
    for (var pageNumber = startPage; pageNumber <= endPage; pageNumber++) {
      var page = <Page>{
        isActive: pageNumber == this.currentPage,
        number: pageNumber,
        text: pageNumber.toString()
      };
      this.pages.push(page);
    }

    // Add links to move between page sets
    if (isMaxSized && !this._config.rotate) {
      if (startPage > 1) {
        var previousPageSet = <Page>{
          isActive: false,
          number: startPage - 1,
          text: '...'
        };
        this.pages.unshift(previousPageSet);
      }

      if (endPage < this.totalPages) {
        var nextPageSet = <Page>{
          isActive: false,
          number: endPage + 1,
          text: '...'
        };
        this.pages.push(nextPageSet);
      }
    }
  }
}

export interface Page {
  isActive: boolean;
  number: number;
  text: string;
}

export interface PageConfig {
  maxSize?: number,
  itemsPerPage?: number;
  boundaryLinks?: boolean;
  directionLinks?: boolean;
  firstText?: string;
  previousText?: string;
  nextText?: string;
  lastText?: string;
  rotate?: boolean;
}
