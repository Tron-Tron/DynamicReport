import React, { Component } from "react";
import Loading from "./Loading";
import "./ListTable.css";
import Modal from "react-bootstrap/Modal";

export default class ModalShowQuery extends Component {
  constructor(props) {
    super(props);
    console.log(props);
    this.state = {
      data: "",
    };

    this.handleHideModalQuery = this.handleHideModalQuery.bind(this);
  }

  async handleHideModalQuery(evt) {
    // Do something ???
    this.props.onClick(evt);
  }
  handleOpenModalQuery = async (evt) => {
    this.setState({ data: "" });
    const data = await this.props.option.getQuery();
    this.setState({ data });
  };
  onShowQuery(stringQuery) {}
  componentDidUpdate = async () => {};
  render() {
    return (
      <Modal
        {...this.props}
        onShow={this.handleOpenModalQuery}
        size="lg"
        aria-labelledby="contained-modal-title-vcenter"
        centered
      >
        <Modal.Header closeButton>
          <Modal.Title id="contained-modal-title-vcenter">
            String Query
          </Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <p>{this.state.data}</p>
        </Modal.Body>
        <Modal.Footer>
          <button onClick={this.handleHideModalQuery}>Close</button>
        </Modal.Footer>
      </Modal>
    );
  }
}
