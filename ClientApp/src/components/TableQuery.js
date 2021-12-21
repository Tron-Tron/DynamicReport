import React, { Component } from "react";
import { RenderReport } from "../services";
import "./TableQuery.css";
const Headers = ["Table", "Field", "Sort", "Show", "Criteria", "Or"];
function uuidv4() {
  return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function (c) {
    var r = (Math.random() * 16) | 0,
      v = c === "x" ? r : (r & 0x3) | 0x8;
    return v.toString(16);
  });
}
export default class TableQuery extends Component {
  constructor(props) {
    super(props);

    this.TableData.push({ key: uuidv4(), data: new Array(Headers.length) });
    this.state = {
      isRefresh: false,
      data: props.data,
    };
  }

  // static defaultProps = {
  //   renderText: (item) => item,
  // };
  // setData = (dataQuery) => {
  //   if (Array.isArray(dataQuery)) {
  //     this.setState({ dataQuery });
  //   }
  // };
  // componentDidMount = async () => {
  //   const data = await GetTableInfos();
  // };

  // generateListTable = () => {
  //   return this.state.dataQuery.map((item, index) => {
  //     return <option key={`key${index}`}>{this.props.renderText(item)}</option>;
  //   });
  // };
  generateHeader = () => {
    return Headers.map((item, index) => {
      return (
        <tr key={`key${index}`}>
          <th className="title-tq">{item}</th>
        </tr>
      );
    });
  };
  setData = (data) => {
    this.setState({ data });
  };
  RenderModel = () => {
    return this.TableData.map((item) => {
      const obj = {};
      Headers.forEach((m, i) => {
        obj[m] = item.data[i];
      });
      return obj;
    });
  };
  generateColumns = (data, indexCol) => {
    const rows = [];
    for (let indexRow = 0; indexRow < data.length; indexRow++) {
      const item = data[indexRow];
      if (indexRow !== 3) {
        this.TableData[indexCol].data[indexRow] = item;
      }
      rows.push(
        <tr key={`key${indexRow}`}>
          {indexRow === 3 ? (
            <td className="tq-show">
              <input defaultValue={item} type="checkbox" checked />
            </td>
          ) : (
            <td>
              <input
                defaultValue={item}
                onChange={(event) => {
                  this.TableData[indexCol].data[indexRow] =
                    event?.target?.value;
                }}
              />
            </td>
          )}
        </tr>
      );
    }
    return rows;
  };
  refLstFields = {};
  refLstTables = {};
  generateRows = (data, indexRow) => {
    return (
      <tr key={`key${indexRow}`} className="title-tq">
        {this.TableData.map((item, indexCol) => {
          return indexRow === 3 ? (
            <td key={item.key} className="tq-show">
              <input
                defaultValue={item.data[indexRow]}
                type="checkbox"
                onChange={(event) => {
                  item.data[indexRow] = event?.target?.checked;
                }}
              />
            </td>
          ) : indexRow === 0 ? (
            <td key={item.key} className="title-tq">
              <TableFieldSelect
                option={{
                  className: "cb-select",
                  defaultValue: item.data[indexRow],
                  onChange: (event) => {
                    const tmpItem = this.state.data[event?.target?.value];
                    item.data[indexRow] = tmpItem.name;
                    this.refLstFields[item.key].setData(
                      tmpItem.fields,
                      this.refLstFields[item.key].UpdateValue
                    );
                  },
                }}
                data={this.state.data}
                // componentDidUpdate={(event) => {
                //   const tmpItem = this.state.data[event?.target?.value];
                //   this.refLstFields[indexCol].setData(tmpItem.fields);
                // }}
                renderOption={(item, index) => ({
                  value: index,
                  name: item.name,
                })}
              />
            </td>
          ) : indexRow === 1 ? (
            <td key={item.key} className="title-tq">
              <TableFieldSelect
                option={{
                  className: "cb-select",
                  defaultValue: item.data[indexRow],
                  onChange: (event) => {
                    item.data[indexRow] = event?.target?.value;
                  },
                }}
                data={this.state.data ? this.state.data.fields : undefined}
                ref={(ref) => (this.refLstFields[item.key] = ref)}
                renderOption={(item, index) => ({
                  value: item,
                  name: item,
                })}
              />
            </td>
          ) : indexRow === 2 ? (
            <td key={item.key} className="title-tq">
              <select
                className="cb-select"
                // defaultValue={item.data[indexRow]}
                onChange={(event) => {
                  item.data[indexRow] = event?.target?.value;
                }}
              >
                <option>none</option>
                <option>ASC</option>
                <option>DESC</option>
              </select>
            </td>
          ) : (
            <td key={item.key} className="title-tq">
              <input
                defaultValue={item.data[indexRow]}
                onChange={(event) => {
                  item.data[indexRow] = event?.target?.value;
                }}
              />
            </td>
          );
        })}
      </tr>
    );
  };
  generateBody = () => {
    const rows = Headers.map(this.generateRows);
    return rows;
  };
  TableData = [];
  componentDidMount() {
    // setInterval(() => {
    //   const data = this.RenderModel();
    //   RenderReport(data);
    // }, 10000);
  }
  AddColumn = () => {
    this.TableData.push({ key: uuidv4(), data: new Array(Headers.length) });
    this.setState({ isRefresh: !this.state.isRefresh });
  };
  DelColumn = (index) => {
    this.TableData.splice(index, 1);
    this.setState({ isRefresh: !this.state.isRefresh });
  };
  generateFooter = () => {
    return (
      <tr>
        {this.TableData.map((item, index) => {
          return (
            <th key={`key${index}`}>
              <button
                onClick={() => this.DelColumn(index)}
                className="btn btn-danger"
              >
                Del
              </button>
            </th>
          );
        })}
      </tr>
    );
  };
  render() {
    return (
      <div className="container-tq">
        <table className="container-tq-left">
          <thead>
            <tr>
              <th className="title-tq">Action</th>
            </tr>
            {this.generateHeader()}
          </thead>
        </table>
        <div className="scrollbar" id="style-1">
          <div className="force-overflow">
            <table className="container-tq-right">
              <thead>
                <tr>
                  <th>
                    <button
                      className="btn btn-success"
                      onClick={this.AddColumn}
                    >
                      Add
                    </button>
                  </th>
                </tr>
              </thead>
              <tbody>{this.generateBody()}</tbody>
              <tfoot>{this.generateFooter()}</tfoot>
            </table>
          </div>
        </div>
      </div>
    );
  }
}

class TableFieldSelect extends Component {
  constructor(props) {
    super(props);
    this.state = {
      data: props.data,
    };
  }
  shouldComponentUpdate = (newProps, newState) => {
    if (this.props.data !== newProps.data) {
      this.state.data = newProps.data;
      return true;
    } else if (newState.data !== this.state.data) {
      return true;
    }
    return false;
  };
  static defaultProps = {
    data: [],
    renderOption: () => ({}),
    option: { onChange: () => {} },
    componentDidUpdate: () => {},
  };
  setData = (data, callback) => {
    this.setState({ data }, callback);
  };
  UpdateValue = () => {
    this.props.option.onChange({ target: this.refSelect });
  };
  componentDidUpdate = () => {
    this.props.componentDidUpdate(this.refSelect);
  };
  render = () => {
    const { renderOption, option } = this.props;
    return (
      <select ref={(ref) => (this.refSelect = ref)} {...option}>
        <option>none</option>
        {this.state.data.map((item, idx) => {
          const tmp = renderOption(item, idx);
          return (
            <option key={`key${idx}`} value={tmp.value}>
              {tmp.name}
            </option>
          );
        })}
      </select>
    );
  };
}
