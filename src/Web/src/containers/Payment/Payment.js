import React, { Component } from "react";
import PropTypes from "prop-types";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import * as paymentActions from "../../redux/modules/payment";
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

class PaymentList extends Component {
  render() {
    const { ids, payments } = this.props;
    return (
      <Container>
        <Row>
          <Col xs="12">
            <div>
              <Table striped responsive hover>
                <thead className="thead-default">
                  <tr>
                    <td>#</td>
                    <td>Customer name</td>
                    <td>Order Id</td>
                    <td>Employee name</td>
                    <td>Money</td>
                    <td>Status</td>
                    <td>Emulating the callback</td>
                  </tr>
                </thead>
                <tbody>
                  {ids.map((id, index) => (
                    <tr key={id}>
                      <td>{index + 1}</td>
                      <td>{payments[id].customerName}</td>
                      <td>{payments[id].orderId}</td>
                      <td>{payments[id].employeeEmail}</td>
                      <td>${payments[id].money}</td>
                      <td>
                        {payments[id].paymentStatus === 0 &&
                          <Badge color="warning">Waiting</Badge>}
                        {payments[id].paymentStatus === 1 &&
                          <Badge color="success">Accepted</Badge>}
                      </td>
                      <td>
                        {payments[id].paymentStatus === 0 &&
                          <Button
                            color="primary"
                            onClick={() =>
                              this.props.callbackFromPaymentGateway(id)}
                          >
                            Callback
                          </Button>}
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

class Payment extends Component {
  componentDidMount() {
    this.props.getPayments();
  }

  render() {
    const { loading, byIds, payments } = this.props.paymentStore;
    if (loading) {
      return <div>Loading...</div>;
    }
    return (
      <Card>
        <CardBlock>
          <br />
          <PaymentList
            ids={byIds}
            payments={payments}
            callbackFromPaymentGateway={this.props.callbackFromPaymentGateway}
          />
        </CardBlock>
      </Card>
    );
  }
}

/*Payment.propTypes = {
  loading: PropTypes.bool.isRequired,
  byIds: PropTypes.array.isRequired,
  payments: PropTypes.object.isRequired,
  getPayments: PropTypes.func.isRequired
};*/

function mapStateToProps(state, ownProps) {
  return {
    paymentStore: state.paymentStore
  };
}

export const mapDispatchToProps = dispatch =>
  bindActionCreators(paymentActions, dispatch);

export default connect(mapStateToProps, mapDispatchToProps)(Payment);
