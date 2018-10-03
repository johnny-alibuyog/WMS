export class PendingList {
  options = [];
  selected = [];

  constructor() {
  }

  activate() {
    this.options = [
      { name: 'First Option', id: "1" },
      { name: 'Second Option', id: "2" },
      { name: 'Third Option', id: "3" }
    ];

    /*
    this.options = [
      { label: 'First Option', value: "1" },
      { label: 'Second Option', value: "2" },
      { label: 'Third Option', value: "3" }
    ];
    */

    this.selected = ["3", "1"];
  }
  
}