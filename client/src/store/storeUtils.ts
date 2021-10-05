export type Action<TPayload> = {
    type: string;
    payload: TPayload;
}

export interface IActionCreator<P> {
  type: string;
  (payload: P): Action<P>;
}
export function actionCreator<P>(type: string): IActionCreator<P> {
  return Object.assign(
    (payload: P) => ({ type, payload }),
    { type },
  );
}
