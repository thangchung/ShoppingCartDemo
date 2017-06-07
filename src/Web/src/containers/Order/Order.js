import React, { Component } from "react";
import PropTypes from "prop-types";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import * as orderActions from "../../redux/modules/order";
import moment from "moment";
import {
  Container,
  Row,
  Col,
  Card,
  CardBlock,
  Table,
  Badge,
  Button
} from "reactstrap";

class OrderList extends Component {
  render() {
    const { ids, orders } = this.props;
    return (
      <Container>
        <Row>
          <Col xs="12">
            <div>
              <Table striped responsive hover>
                <thead className="thead-default">
                  <tr>
                    <td>#</td>
                    <td>Order Id</td>
                    <td>Order Date</td>
                    <td>Status</td>
                  </tr>
                </thead>
                <tbody>
                  {ids.map((id, index) => (
                    <tr key={id}>
                      <td>{index + 1}</td>
                      <td>{orders[id].id}</td>
                      <td>
                        {moment(orders[id].orderDate).format(
                          "dddd, MMMM Do YYYY, h:mm:ss a"
                        )}
                      </td>
                      <td>
                        {orders[id].orderStatus === 0 && <Badge>New</Badge>}
                        {orders[id].orderStatus === 1 &&
                          <Badge color="warning">Processing</Badge>}
                        {orders[id].orderStatus === 2 &&
                          <Badge color="danger">WaitingPayment</Badge>}
                        {orders[id].orderStatus === 3 &&
                          <Badge color="success">Paid</Badge>}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </Table>
            </div>
          </Col>
        </Row>
      </Container>
    );
  }
}

class Order extends Component {
  componentDidMount() {
    this.props.getOrders();
  }

  render() {
    const { loading, byIds, orders } = this.props.orderStore;
    if (loading) {
      return <div>Loading...</div>;
    }
    return (
      <Card>
        <CardBlock>
          <br />
          <OrderList ids={byIds} orders={orders} />
        </CardBlock>
      </Card>
    );
  }
}

function mapStateToProps(state, ownProps) {
  return {
    orderStore: state.orderStore
  };
}

export const mapDispatchToProps = dispatch =>
  bindActionCreators(orderActions, dispatch);

export default connect(mapStateToProps, mapDispatchToProps)(Order);
