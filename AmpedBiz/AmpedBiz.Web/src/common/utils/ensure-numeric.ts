export let ensureNumeric = (value: any) => {
  var number = parseFloat(value);

  if (isNaN(number))
    return 0;

  if (!isFinite(number))
    return 0;

  return number;
}