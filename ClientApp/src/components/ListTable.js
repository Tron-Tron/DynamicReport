import React, { Component } from "react";
import Loading from "./Loading";
import "./ListTable.css";
export default class ListTable extends Component {
  constructor(props) {
    super(props);

    this.state = {
      data: props.data,
    };
  }
  static defaultProps = {
    onclickItem: ({ event, item, index }) => {
      console.log("default:", item);
    },
    renderText: (item) => item,
  };
  setData = (data) => {
    if (Array.isArray(data)) {
      this.setState({ data });
    }
  };
  // componentDidMount = async () => {
  //   const data = await GetTableInfos();
  // };

  generateList = () => {
    return this.state.data.map((item, index) => {
      return (
        <li
          onClick={(event) => this.props.onclickItem({ event, item, index })}
          key={`key${index}`}
        >
          {this.props.renderText(item, index)}
        </li>
      );
    });
  };
  render() {
    return this.state.data ? (
      <ol className="table-list">{this.generateList()}</ol>
    ) : (
      <Loading />
    );
  }
}
