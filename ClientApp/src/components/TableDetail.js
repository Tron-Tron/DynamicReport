import React, { Component } from "react";
import ListTable from "./ListTable";
import "./TableDetail.css";
export default class TableDetail extends Component {
  constructor(props) {
    super(props);
  }
  dataTable = {};
  setData = (data) => {
    this.dataTable = data;
    console.log(data);
    this.refLstFields.setData(data.fields || []);
    this.refLstRelationShips.setData(data.relationShips || []);
  };
  render() {
    console.log(this.state);
    return (
      <div className="container-td">
        <div className="container-td-left round border">
          <h5 className="text-center">Fields</h5>
          <ListTable
            data={[]}
            renderText={(item, index) =>
              `${item} - ${
                Array.isArray(this.dataTable.types)
                  ? this.dataTable.types[index]
                  : ""
              }`
            }
            ref={(ref) => (this.refLstFields = ref)} //controll class
            onclickItem={this.onclickItemTable}
          />
        </div>
        <div className="container-td-right round border">
          <h5 className="text-center">RelationShips</h5>
          <ListTable
            data={[]}
            ref={(ref) => (this.refLstRelationShips = ref)} //controll class
            onclickItem={this.onclickItemTable}
          />
        </div>
      </div>
    );
  }
}
