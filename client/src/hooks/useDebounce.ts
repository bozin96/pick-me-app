import { useEffect, useState } from 'react';
import { debounceTime, Subject } from 'rxjs';

function useDebounce<T = any>(time: number, defaultValue: T): [T, (v: T) => void] {
    const [value, setValue] = useState<T>(defaultValue);
    const [value$] = useState(() => new Subject<T>());
    useEffect(() => {
      const sub = value$.pipe(debounceTime(time)).subscribe(setValue);
      return () => sub.unsubscribe();
    }, [time, value$]);
   return [value, (v) => value$.next(v)];
  }

  export default useDebounce;
