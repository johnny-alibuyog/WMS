export type StageDefinition<TStatus, TAggregate> = {
  allowedTransitions: TStatus[];
  allowedModifications: TAggregate[];
}
