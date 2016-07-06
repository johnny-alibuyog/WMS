import {UnitOfMeasure} from './unit-of-measure';

export interface UnitOfMeasureClass {
  id?: string;
  name?: string;
  units?: UnitOfMeasure[];
}

export interface UnitOfMeasureClassPageItem {
  id?: string;
  name?: string;
}