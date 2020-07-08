import { Pipe, PipeTransform } from '@angular/core';
import { isObservable, of, Observable } from 'rxjs';
import { map, startWith, catchError } from 'rxjs/operators';

@Pipe({
  name: 'withLoading'
})
export class WithLoadingPipe implements PipeTransform {

  public transform(val: any): any {
    return isObservable(val) ? this.wrapObservableWithLoading(val) : val;
  }

  private wrapObservableWithLoading(observable: Observable<any>): Observable<any> {
    return observable.pipe(
      map((value: any) => ({ loading: false, value })),
      startWith({ loading: true }),
      catchError(error => of({ loading: false, error }))
    );
  }

}