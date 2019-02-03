export interface Lookup<TId> {
  id: TId;
  name: string;
}

export const getName = <TId>(lookup: Lookup<TId>) => lookup && lookup.name || '';
