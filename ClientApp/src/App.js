import React, { Component } from "react";
import ListTable from "./components/ListTable";
import Loading from "./components/Loading";
import TableDetail from "./components/TableDetail";
import ModalShowQuery from "./components/ModalShowQuery";

import { GetStringQuery, GetTableInfos, RenderReport } from "./services";

import "./custom.css";
import TableQuery from "./components/TableQuery";
let relations;
export default class App extends Component {
  constructor(props) {
    super(props);

    this.state = {
      modalShow: false,
    };
    this.setModalShow = this.setModalShow.bind(this);
    this.setModalHide = this.setModalHide.bind(this);
  }

  setModalHide(evt) {
    this.setState({
      modalShow: false,
    });
  }
  static displayName = App.name;
  render1 = () => {
    return <Loading />;
  };
  onclickItemTable = ({ event, item, index }) => {
    this.refTableDetail.setData(item);
  };
  componentDidMount = async () => {
    const dataTableList = await GetTableInfos();
    this.data = dataTableList;
    console.log("data list table", dataTableList);
    this.refLstTable.setData(dataTableList);
    this.refTableQuery.setData(dataTableList);
  };

  TravelRelations = (dataRendered) => {
    const nameTables = [...new Set(dataRendered.map((item) => item.Table))];
    const { data } = this;
    console.log("data Traveler", data);
    console.log("nameTables", nameTables.length);
    let count = 0;
    const queue = [nameTables[0]];
    const tempQueue = [];
    console.log("queue", queue);
    console.log("data", data);
    while (queue.length > 0) {
      count++;
      const u = queue.shift();
      tempQueue.push(u);
      console.log("tempQueue", tempQueue);

      const table = data.find((x) => x.name === u);
      console.log("table", table);

      relations = table?.relationShipInfos?.filter((x) =>
        nameTables.some((y) => x.tableFK === y)
      );
      // console.log("relations", relations);
      // if (relations.length === 0) {
      //   alert("Các table không có quan hệ. Vui lòng kiểm tra lại!!");
      //   break;
      // }
if(relations){
  relations.forEach((item) => {
    const dataTable = dataRendered.filter((x) => x.Table === item.tableFK);
    dataTable.forEach(
      (table) =>
        (table.JoinOn = `join ${item.tableFK} on ${item.tablePK}.${item.pk} = ${item.tableFK}.${item.fk}`)
    );
    console.log("dataTable", dataTable);
  });

  queue.push(...relations.map((item) => item.tableFK));
}
     

      if (count === nameTables.length) {
        break;
      }
    }
    console.log("ra day k");
  };
  setModalShow(evt) {
    this.setState({
      modalShow: true,
    });
  }
  getQuery = async () => {
    const dataRender = this.refTableQuery.RenderModel();
    //  console.log("dataRendered", dataRender);
    if (!Array.isArray(dataRender) || dataRender.length < 1) return;

    this.TravelRelations(dataRender);
    let flag = true;
    console.log("vao day k");
    dataRender.forEach((value) => {
      if (value.Table === undefined) {
        alert("Table không được rỗng");
        flag = false;
      }
      if (value.Field === undefined) {
        alert("Field không được rỗng");
      }
    });
    if (flag) {
      return await GetStringQuery(dataRender);
    }
  };
  getReport = async () => {
    const dataRender = this.refTableQuery.RenderModel();
    if (!Array.isArray(dataRender) || dataRender.length < 1) return;
    this.TravelRelations(dataRender);
    let flag = true;

    dataRender.forEach((value) => {
      if (value.Table === undefined) {
        alert("Table không được rỗng");
        flag = false;
      }
      if (value.Field === undefined) {
        alert("Field không được rỗng");
        flag = false;
      }
    });
    if (flag) {
      return await RenderReport(dataRender);
    }
  };
  onCreateReport = async () => {
    const data = await this.getReport();
    if (data) {
      window.open(`/Pdfs/${data}`);
    }
  };

  render() {
    return (
      <div className="m-container">
        <div className="m-container-left round shadow">
          <h5 className="text-center">Table List</h5>
          <ListTable
            renderText={(item) => item.name}
            ref={(ref) => (this.refLstTable = ref)} //controll class
            onclickItem={this.onclickItemTable}
          />
        </div>
        <div className="m-container-right">
          <div className="m-container-top round">
            <h5 className="text-center">Table Detail</h5>
            <TableDetail ref={(ref) => (this.refTableDetail = ref)} />
          </div>
          <div className="m-container-bottom round">
            <h5 className="text-center">Table Query</h5>
            <TableQuery
              renderText={(item) => item.name}
              ref={(ref) => (this.refTableQuery = ref)}
            />
          </div>
          <div className="d-flex justify-content-end">
            <button
              variant="primary"
              onClick={this.setModalShow}
              className="btn btn-success"
              style={{ margin: "5px" }}
            >
              SHOW SQL QUERY
            </button>
            <ModalShowQuery
              show={this.state.modalShow}
              onClick={this.setModalHide}
              option={{ getQuery: this.getQuery }}
            />
            <button
              onClick={this.onCreateReport}
              className="btn btn-success"
              style={{ margin: "5px" }}
            >
              CREATE REPORT
            </button>
          </div>
        </div>
      </div>
    );
  }
}
