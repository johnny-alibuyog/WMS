import * as Enumerable from 'linq';

export class App {
  message = 'Hello World!';
  count = Enumerable
    .range(0, 20)
    .aggregate(0, 
      (prev, current) => prev + current
    );

}
