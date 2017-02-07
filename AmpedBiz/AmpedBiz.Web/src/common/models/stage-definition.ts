export interface StageDefinition<TStatus, TAggregate> {
  allowedTransitions: TStatus[];
  allowedModifications: TAggregate[];
}