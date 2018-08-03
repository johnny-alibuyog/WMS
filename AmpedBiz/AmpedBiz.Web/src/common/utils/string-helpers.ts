export let isNullOrWhiteSpace = (value: string) => {
  if (value == null)
    return true;

  if (value.trim() === '')
    return true;

  return false;
}