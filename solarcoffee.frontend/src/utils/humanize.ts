import moment from "moment";

const date = (dateFull: Date) => {
  return moment(dateFull).format("MMMM Do YYYY");
};

const price = (num: number) => {
  if (isNaN(num)) {
    return "-";
  }
  return "$ " + num.toFixed(2);
};

export { date, price };
