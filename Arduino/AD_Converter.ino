void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
}

void loop() {
  // put your main code here, to run repeatedly:
  int analogVal = 0;
  
  analogVal = analogRead(A0);
  float digitalVal = (float)(5 * analogVal)/1024;
  Serial.print(analogVal);
  Serial.print(",");
  Serial.println(digitalVal);
  
  delay(100);
}
